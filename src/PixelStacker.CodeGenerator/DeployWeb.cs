using System;
using FluentFTP;
using Renci.SshNet;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PixelStacker.CodeGenerator
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
        private static readonly Stopwatch sw = new Stopwatch();
        

        // Config
        private const string CHOWN = "taylorlove:psacln";
        private const string SERVICE_NAME = "pixelstacker.web.net.service";
        private static string FTP_HOST;
        private static string FTP_USERNAME;
        private static string FTP_PASSWORD;
        private static string SSH_USERNAME;
        private static string SSH_PASSWORD;
        
        // Local file paths
        private static string SolutionRootDir = AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\PixelStacker.CodeGenerator\\bin\\" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        private static string UPLOAD_API_FROM_DIR = Path.Combine(SolutionRootDir, "PixelStacker.Web.Net", "bin", "Release", "net6.0", "publish");
        private static string UPLOAD_UI_FROM_DIR = Path.Combine(SolutionRootDir, "PixelStacker.Web.Net", "PixelStacker.Web.React", "dist");

        // Remote file paths
        private const string SSH_WORKINGDIR_ROOT = "/var/www/vhosts/taylorlove.info";
        private const string DEPLOY_API_TO_REMOTE_DIR = "./services/pixelstacker.web.net-deploy";
        private const string MOVE_DEPLOYED_API_TO_REMOTE_DIR = "./services/pixelstacker.web.net";
        private const string MAIN_API_ARTIFACT_TO_CHMOD = "PixelStacker.Web.Net";

        private const string DEPLOY_UI_TO_REMOTE_DIR = "./httpdocs/projects/pixelstacker.web.react-deploy";
        private const string MOVE_DEPLOYED_UI_TO_REMOTE_DIR = "./httpdocs/projects/pixelstacker.web.react";

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
        public void DeployUIToWebServer()
        {
            sw.Start();
            DeployWeb.sw.WriteLine("Connecting FTP ....");
            if (!MOVE_DEPLOYED_UI_TO_REMOTE_DIR.StartsWith("./") || MOVE_DEPLOYED_UI_TO_REMOTE_DIR.EndsWith("/")) throw new ArgumentException("INVALID ORIG_DIR");
            if (!DEPLOY_UI_TO_REMOTE_DIR.StartsWith("./") || DEPLOY_UI_TO_REMOTE_DIR.EndsWith("/")) throw new ArgumentException("INVALID DEPLOY_DIR");

            using (SshClient ssh = new SshClient(FTP_HOST, SSH_USERNAME, SSH_PASSWORD))
            {
                // Improve security by removing weak hash algorithms
                // https://github.com/Pangamma/PixelStacker/security/dependabot/1
                ssh.ConnectionInfo.KeyExchangeAlgorithms.Remove("curve25519-sha256");
                ssh.ConnectionInfo.KeyExchangeAlgorithms.Remove("curve25519-sha256@libssh.org");
                ssh.Connect();

                string result = "";
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; [ -d {DEPLOY_UI_TO_REMOTE_DIR} ] && echo exists || echo does not exist;").Result.Trim();

                // Remove old dir if it still  exists.
                bool deployDirStillExists = result == "exists";
                if (deployDirStillExists)
                {
                    result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT};  rm -rf {DEPLOY_UI_TO_REMOTE_DIR};").Result;
                }

                var ftp = GetFtpClientFromPool();
                ftp.CreateDirectory(DEPLOY_API_TO_REMOTE_DIR);
                ReturnFtpClientToPool(ftp);

                RecursiveZipUpload(UPLOAD_UI_FROM_DIR, DEPLOY_UI_TO_REMOTE_DIR, ssh);

                DeployWeb.sw.WriteLine("Deprecating ancient deployment");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; mv {MOVE_DEPLOYED_UI_TO_REMOTE_DIR} {MOVE_DEPLOYED_UI_TO_REMOTE_DIR}-old;").Result;

                DeployWeb.sw.WriteLine("Swapping to new deployment");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; mv {DEPLOY_UI_TO_REMOTE_DIR} {MOVE_DEPLOYED_UI_TO_REMOTE_DIR};").Result;

                //DeployWeb.sw.WriteLine("Setting permissions on the main executable.");
                //result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}/{MOVE_DEPLOYED_UI_TO_REMOTE_DIR}; chmod 754 ./{MAIN_UI_ARTIFACT_TO_CHMOD};").Result;

                DeployWeb.sw.WriteLine("Cleaning up old files");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT};  rm -rf {MOVE_DEPLOYED_UI_TO_REMOTE_DIR}-old;").Result;
            }

            clients.ForEach(x => x.Dispose());
            DeployWeb.sw.WriteLine("Finished!");
            sw.Stop();
        }

        [TestMethod]
        public void DeployApiToWebServer()
        {
            sw.Start();
            DeployWeb.sw.WriteLine("Connecting FTP ....");
            if (!MOVE_DEPLOYED_API_TO_REMOTE_DIR.StartsWith("./") || MOVE_DEPLOYED_API_TO_REMOTE_DIR.EndsWith("/")) throw new ArgumentException("INVALID ORIG_DIR");
            if (!DEPLOY_API_TO_REMOTE_DIR.StartsWith("./") || DEPLOY_API_TO_REMOTE_DIR.EndsWith("/")) throw new ArgumentException("INVALID DEPLOY_DIR");

            using (SshClient ssh = new SshClient(FTP_HOST, SSH_USERNAME, SSH_PASSWORD))
            {
                // Improve security by removing weak hash algorithms
                // https://github.com/Pangamma/PixelStacker/security/dependabot/1
                ssh.ConnectionInfo.KeyExchangeAlgorithms.Remove("curve25519-sha256");
                ssh.ConnectionInfo.KeyExchangeAlgorithms.Remove("curve25519-sha256@libssh.org");
                ssh.Connect();

                string result = "";
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; [ -d {DEPLOY_API_TO_REMOTE_DIR} ] && echo exists || echo does not exist;").Result.Trim();

                // Remove old dir if it still  exists.
                bool deployDirStillExists = result == "exists";
                if (deployDirStillExists)
                {
                    result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT};  rm -rf {DEPLOY_API_TO_REMOTE_DIR};").Result;
                }

                var ftp = GetFtpClientFromPool();
                ftp.CreateDirectory(DEPLOY_API_TO_REMOTE_DIR);
                ReturnFtpClientToPool(ftp);

                RecursiveZipUpload(UPLOAD_API_FROM_DIR, DEPLOY_API_TO_REMOTE_DIR, ssh);

                DeployWeb.sw.WriteLine("Stopping old service");
                string svcStopResult = ssh.CreateCommand($"systemctl stop {SERVICE_NAME}").Execute();

                DeployWeb.sw.WriteLine("Deprecating ancient deployment");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; mv {MOVE_DEPLOYED_API_TO_REMOTE_DIR} {MOVE_DEPLOYED_API_TO_REMOTE_DIR}-old;").Result;

                DeployWeb.sw.WriteLine("Swapping to new deployment");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; mv {DEPLOY_API_TO_REMOTE_DIR} {MOVE_DEPLOYED_API_TO_REMOTE_DIR};").Result;

                DeployWeb.sw.WriteLine("Setting permissions on the main executable.");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}/{MOVE_DEPLOYED_API_TO_REMOTE_DIR}; chmod 754 ./{MAIN_API_ARTIFACT_TO_CHMOD};").Result;

                DeployWeb.sw.WriteLine("Starting new service");
                string svcStartResult = ssh.CreateCommand($"systemctl start {SERVICE_NAME}").Execute();

                DeployWeb.sw.WriteLine("Cleaning up old files");
                result = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT};  rm -rf {MOVE_DEPLOYED_API_TO_REMOTE_DIR}-old;").Result;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localDirToUpload">Path.Combine(SolutionRootDir, "PixelStacker.Web.Net", "bin", "Release", "net6.0", "publish");</param>
        /// <param name="remoteDirToUploadTo">./services/pixelstacker.web.net-deploy</param>
        /// <param name="ssh"></param>
        private static void RecursiveZipUpload(string localDirToUploadPath, string remoteDirToUploadTo, SshClient ssh)
        {
            DirectoryInfo localDirToUpload = new DirectoryInfo(localDirToUploadPath);
            DeployWeb.sw.WriteLine("\n-----------------  Zipping files  -----------------------");
            var localZipFilePath = Path.Combine(localDirToUpload.Parent.FullName, "deployment.zip");
            var remoteZipFilePath = remoteDirToUploadTo + "/deployment.zip";

            if (File.Exists(localZipFilePath))
            {
                File.Delete(localZipFilePath);
            }

            ZipFile.CreateFromDirectory(localDirToUpload.FullName, localZipFilePath, CompressionLevel.Optimal, false);
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
                var cmdResult = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}/{remoteDirToUploadTo};  unzip deployment.zip;").Result;

                DeployWeb.sw.WriteLine("Removing zip file on remote server.");
                var cmdResult2 = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}/{remoteDirToUploadTo}; rm ./deployment.zip;").Result;

                DeployWeb.sw.WriteLine($"Chowning the unzipped files to {CHOWN}");
                var cmdResult3 = ssh.RunCommand($"cd {SSH_WORKINGDIR_ROOT}; chown -R {CHOWN} {remoteDirToUploadTo};").Result;
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
    }
}