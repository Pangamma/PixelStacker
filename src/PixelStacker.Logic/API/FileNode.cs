using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.API
{
    public class FileNode
    {
        public byte[] Data { get; set; }
        public string ContentType { get; set; }
        public string SuggestedFileName { get; set; }

        public FileNode(byte[] data, string ct)
        {
            this.Data = data;
            this.ContentType = ct;
        }
    }
}
