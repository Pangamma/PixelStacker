using System;

namespace PixelStacker.Logic.IO.Config
{
    public class UpdateSettings
    {
        public DateTime? LastChecked { get; set; } = null;
        public string SkipNotifyIfVersionIs { get; set; } = Constants.Version;
    }
}
