using Newtonsoft.Json;
using PixelStacker.Core.Model.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Core.IO.Config
{
    public class Options
    {
        [JsonIgnore]
        private IOptionsProvider StorageProvider;
        public Options(IOptionsProvider storage)
        {
            this.StorageProvider = storage;
        }

        public UpdateSettings UpdateSettings { get; set; } = new UpdateSettings();
    }
}
