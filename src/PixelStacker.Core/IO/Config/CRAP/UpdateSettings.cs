using PixelStacker.Core.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Core.IO.Config
{
    public class UpdateSettings
    {
        public DateTime? LastChecked { get; set; } = null;
        public string SkipNotifyIfVersionIs { get; set; } = Constants.Version;
    }
}
