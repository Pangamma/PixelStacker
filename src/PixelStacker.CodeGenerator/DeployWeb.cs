using System;
using FluentFTP;
using Renci.SshNet;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;
using System.IO.Compression;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtCoffee.Publish
{
    public static class StopwatchExtension
    {
        public static string ToPrefix(this Stopwatch sw)
        {
            return Math.Round(sw.Elapsed.TotalSeconds, 1).ToString().PadLeft(3, '0') + "s    ";
        }

        public static void WriteLine(this Stopwatch sw, string msg)
        {
            string prefix = Math.Round(sw.Elapsed.TotalSeconds, 1).ToString().PadLeft(3, '0') + "s    ";
            Console.WriteLine(prefix + msg);
        }
    }

    [TestClass]
    [TestCategory("Tools")]
    public class DeployWeb
    {
        //cp -a ./mvc.lumengaming.com/. ./mvc.lumengaming.com-deploy/
        private const string DEPLOY_DIR = "./services/pixelstacker.web.net-deploy";
        private const string ORIG_DIR = "./services/pixelstacker.web.net";
        private const string SSH_WORKINGDIR_ROOT = "/var/www/vhosts/taylorlove.info";
        private const string SERVICE_NAME = "pixelstacker.web.net.service";
        private const string FTP_MAIN_TO_CHMOD = "PixelStacker.Web.Net";
        private const string CHOWN = "taylorlove:psacln";
        private static string FTP_HOST;
        private static string FTP_USERNAME;
        private static string FTP_PASSWORD;
        private static string SSH_USERNAME;
        private static string SSH_PASSWORD;
        private const bool IS_ZIPPING_ENABLED = true;
        private static readonly Stopwatch sw = new Stopwatch(); 
        private static string RootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.CodeGenerator\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        private static string UPLOAD_FROM_DIR = Path.Combine(RootDir, "PixelStacker.Web.Net", "bin", "Release", "net6.0", "publish");

        public DeployWeb()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<DeployWeb>()
                .Build();
            FTP_USERNAME = config["FTP_USERNAME"];
            FTP_PASSWORD = config["FTP_PASSWORD"];
            SSH_USERNAME = config["SSH_USERNAME"];
            SSH_PASSWORD = config["SSH_PASSWORD"];
            FTP_HOST = config["FTP_HOST"];
        }


        [TestMethod]
        public void DeployToWebServer()
        {
            sw.Start();
            DeployWeb.sw.WriteLine("Connecting FTP ....");
            if (!ORIG_DIR.StartsWith("./") || ORIG_DIR.EndsWith("/")) throw new ArgumentException("INVALID ORIG_DIR");
            if (!DEPLOY_DIR.StartsWith("./") || DEPLOY_DIR.EndsWith("/")) throw new ArgumentException("INVALID DEPLOY_DIR");

            using (SshClient ssh = new SshClient(FTP_HOST, SSH_USERNAME, SSH_PASSWORD))
            {
                // Improve security by removing weak hash algorithms
                // https://github.com/Pangamma/PixelStacker/security/dependabot/1
                ssh.ConnectionInfo.KeyExchangeAlgorithms.Remove("curve25519-sha256");
                ssh.ConnectionInfo.KeyExchangeAlgorithms.Remove("curve25519-sha256@libssh.org");
                ssh.Connect();

                string result = "";
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; [ -d {DEPLOY_DIR} ] && echo exists || echo does not exist;").Result.Trim();

                // Remove old dir if it still  exists.
                bool deployDirStillExists = result == "exists";
                if (deployDirStillExists)
                {
                    result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT};  rm -rf {DEPLOY_DIR};").Result;
                }

                var ftp = GetFtpClientFromPool();
                ftp.CreateDirectory(DEPLOY_DIR);
                ReturnFtpClientToPool(ftp);

                var dir = new DirectoryInfo(UPLOAD_FROM_DIR);
                if (IS_ZIPPING_ENABLED)
                {
                    RecursiveZipUpload(dir, ssh);
                } 
                else
                {
#pragma warning disable CS0162 // Unreachable code detected
                    RecursiveUpload(dir, DEPLOY_DIR);
#pragma warning restore CS0162 // Unreachable code detected
                }

                DeployWeb.sw.WriteLine("Stopping old service");
                string svcStopResult = ssh.CreateCommand($"systemctl stop {SERVICE_NAME}").Execute();

                DeployWeb.sw.WriteLine("Deprecating ancient deployment");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; mv {ORIG_DIR} {ORIG_DIR}-old;").Result;

                DeployWeb.sw.WriteLine("Swapping to new deployment");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; mv {DEPLOY_DIR} {ORIG_DIR};").Result;

                DeployWeb.sw.WriteLine("Cleaning up old files");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT};  rm -rf {ORIG_DIR}-old;").Result;

                DeployWeb.sw.WriteLine("Setting permissions on the main executable.");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}/{ORIG_DIR}; chmod 754 ./{FTP_MAIN_TO_CHMOD};").Result;

                DeployWeb.sw.WriteLine("Starting new service");
                string svcStartResult = ssh.CreateCommand($"systemctl start {SERVICE_NAME}").Execute();
            }

            clients.ForEach(x => x.Dispose());
            DeployWeb.sw.WriteLine("Finished!");
            sw.Stop();
        }


        private readonly static int MAX_CONCURRENT_UPLOADS = 10;
        private static int CUR_CONCURRENT_UPLOADS = 0;
        private readonly static List<FtpClient> clients = new List<FtpClient>();
        private static FtpClient GetFtpClientFromPool()
        {
            try
            {
                lock (clients)
                {
                    if (CUR_CONCURRENT_UPLOADS <= MAX_CONCURRENT_UPLOADS)
                    {
                        if (clients.Count > 0)
                        {
                            CUR_CONCURRENT_UPLOADS++;
                            var rt = clients[0];
                            clients.RemoveAt(0);
                            return rt;
                        }
                        else
                        {
                            CUR_CONCURRENT_UPLOADS++;
                            var ftp = new FtpClient(FTP_HOST, 21, FTP_USERNAME, FTP_PASSWORD)
                            {
                                UploadRateLimit = 0,
                                SocketKeepAlive = true,
                                EncryptionMode = FtpEncryptionMode.Explicit,
                                //SslProtocols = System.Security.Authentication.SslProtocols.Tls
                                DataConnectionEncryption = true,

                                // Might potentially get MITM'd from an intruder on the network,
                                // but at least it's better than an unencrypted connection over
                                // the wider internet.
                                ValidateAnyCertificate = true,
                            };

                            ftp.Connect();
                            return ftp;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return null;
        }

        private static void ReturnFtpClientToPool(FtpClient ftp)
        {
            CUR_CONCURRENT_UPLOADS--;
            lock (clients)
            {
                clients.Add(ftp);
            }
        }

        private static readonly object padlock = new {};
        private static void SetConsoleLine(int row, string text, ConsoleColor? color = null)
        {
            lock (padlock)
            {
                if (row == -1)
                {
                    DeployWeb.sw.WriteLine(text);
                    return;
                }

                int orig = Console.CursorTop;
                ConsoleColor cOrig = Console.ForegroundColor;
                Console.SetCursorPosition(0, row);
                Console.Write(new string(' ', Console.WindowWidth));

                if (color != null) Console.ForegroundColor = color.Value;
                Console.SetCursorPosition(0, row);
                DeployWeb.sw.WriteLine(text);
                Console.SetCursorPosition(0, orig);

                if (color != null) Console.ForegroundColor = cOrig;
            }
        }

        private static void RecursiveZipUpload(DirectoryInfo dir, SshClient ssh)
        {
            DeployWeb.sw.WriteLine("\n-----------------  Zipping files  -----------------------");
            var localZipFilePath = Path.Combine(dir.Parent.FullName, "deployment.zip");
            var remoteZipFilePath = DEPLOY_DIR + "/deployment.zip";

            if (File.Exists(localZipFilePath))
            {
                File.Delete(localZipFilePath);
            }

            ZipFile.CreateFromDirectory(dir.FullName, localZipFilePath, CompressionLevel.Optimal, false);
            var ftp = GetFtpClientFromPool();
            while (ftp == null)
            {
                Console.WriteLine(DeployWeb.sw.ToPrefix() + $"Waiting for FTPClient.", ConsoleColor.Red);
                ftp = GetFtpClientFromPool();
            }

            try
            {
                var file = new FileInfo(localZipFilePath);
                var kb = file.Length / 1024;
                Console.WriteLine(DeployWeb.sw.ToPrefix() + $"Uploading {kb}kb... {localZipFilePath}", ConsoleColor.Yellow);
                ftp.UploadFile(localZipFilePath, remoteZipFilePath, FluentFTP.FtpRemoteExists.Overwrite, true, FtpVerify.None);

                DeployWeb.sw.WriteLine("Unzipping...");
                var cmdResult = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}/{DEPLOY_DIR};  unzip deployment.zip;").Result;

                DeployWeb.sw.WriteLine("Removing zip file on remote server.");
                var cmdResult2 = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}/{DEPLOY_DIR}; rm ./deployment.zip;").Result;

                DeployWeb.sw.WriteLine($"Chowning the unzipped files to {CHOWN}");
                var cmdResult3 = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; chown -R {CHOWN} {DEPLOY_DIR};").Result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
            ReturnFtpClientToPool(ftp);

            // Clean up the useless file afterwards.
            if (File.Exists(localZipFilePath))
            {
                DeployWeb.sw.WriteLine("Cleaning up temporary file");
                File.Delete(localZipFilePath);
            }
        }


        private static void RecursiveUpload(DirectoryInfo dir, string deployFolder)
        {
            var toUpload = new Dictionary<string, string>();
            RecursiveUploadInit(dir, deployFolder, ref toUpload);
            Console.Write("\n-----------------  Uploading Files  -----------------------");

            int cursorPos;

            try
            {
                cursorPos = Console.CursorTop;
            }
            catch (IOException)
            {
                cursorPos = -1; // no console supported.
            }

            toUpload.ToList().AsParallel<KeyValuePair<string, string>>()
                .WithDegreeOfParallelism(Math.Min(MAX_CONCURRENT_UPLOADS - 1, toUpload.Count))
                .ForAll(kvp =>
                {
                    int curConsoleLine = cursorPos != -1 ? Interlocked.Increment(ref cursorPos) : -1;
                    long kb = (new FileInfo(kvp.Key)).Length / 1024;
                    if (cursorPos != -1) SetConsoleLine(curConsoleLine, $"Starting {kb}kb... {kvp.Value}");

                    var ftp = GetFtpClientFromPool();
                    while (ftp == null)
                    {
                        Console.SetCursorPosition(0, curConsoleLine);
                        SetConsoleLine(curConsoleLine, $"Waiting for FTPClient. {kvp.Value}", ConsoleColor.Red);
                        Task.Delay(150);
                        ftp = GetFtpClientFromPool();
                    }

                    try
                    {
                        SetConsoleLine(curConsoleLine, $"Uploading {kb}kb... {kvp.Value}", ConsoleColor.Yellow);
                        ftp.UploadFile(kvp.Key, kvp.Value, FluentFTP.FtpRemoteExists.Overwrite, true, FtpVerify.None);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                    ReturnFtpClientToPool(ftp);
                    if (cursorPos != -1) SetConsoleLine(curConsoleLine, $"Finished {kb}kb... {kvp.Value}", ConsoleColor.Green);
                });

            if (cursorPos != -1) Console.SetCursorPosition(0, cursorPos + 1);
            DeployWeb.sw.WriteLine("\n--------  Switching deployment slots  ---------------------");
        }

        private static void RecursiveUploadInit(DirectoryInfo dir, string deployFolder, ref Dictionary<string, string> toUpload)
        {
            Console.WriteLine("Scanning files in... {0}", deployFolder.Replace(DEPLOY_DIR, ""));
            var filesInDir = dir.EnumerateFiles().Select(x => x.FullName);
            var dirsInDir = dir.EnumerateDirectories();

            var ftpO = GetFtpClientFromPool();
            if (!ftpO.DirectoryExists(deployFolder))
            {
                ftpO.CreateDirectory(deployFolder);
            }
            ReturnFtpClientToPool(ftpO);

            foreach (var fi in filesInDir)
            {
                string fiFi = deployFolder + "/" + fi.Split('\\').Last();
                toUpload.Add(fi, fiFi);
            }

            foreach (var dirr in dirsInDir)
            {
                RecursiveUploadInit(dirr, deployFolder + "/" + dirr.Name, ref toUpload);
            }
        }
    }
}