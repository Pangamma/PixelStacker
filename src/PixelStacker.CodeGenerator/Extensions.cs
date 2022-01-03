using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.CodeGenerator
{
    public static class Extensions
    {
        public static void GetFilesRecursively(this DirectoryInfo dir, Action<FileInfo> actn)
        {
            if (!dir.Exists) return;
            foreach(var fi in dir.GetFiles())
            {
                actn.Invoke(fi);
            }

            foreach(var di in dir.GetDirectories())
            {
                GetFilesRecursively(di, actn);
            }
        }
    }
}
