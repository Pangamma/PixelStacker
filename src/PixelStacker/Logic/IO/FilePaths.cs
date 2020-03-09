using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO
{
    public static class FilePaths
    {
        public static string AppDataDir
        {
            get
            {
                try
                {
                    string appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    string pixelStackerPath = Path.Combine(appPath, "PixelStacker");

                    if (!Directory.Exists(pixelStackerPath))
                    {
                        Directory.CreateDirectory(pixelStackerPath);
                    }

                    return pixelStackerPath;
                }
                catch (Exception ex)
                {
                    // not really all that important...
                    Console.Error.WriteLine(ex);
                }

                return "";
            }
        }

        public static string ColorProfilesPath
        {
            get
            {
                string path = Path.Combine(FilePaths.AppDataDir, "color-profiles");
                
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
    }
}
