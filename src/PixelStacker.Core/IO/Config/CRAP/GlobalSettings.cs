using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Core.IO.Config
{
    public class GlobalSettings
    {
        /// <summary>
        /// Tracks how long ago an update check was performed, and if user has decided to skip
        /// the current latest version.
        /// </summary>
        public UpdateSettings UpdateSettings { get; set; }

        /// <summary>
        /// The locale for the program
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Toggle through the konami code. (up up down down left right left right B A Enter)
        /// Enables advanced materials and other advanced modes.
        /// </summary>
        public bool IsAdvancedModeEnabled { get; set; }
    }
}
