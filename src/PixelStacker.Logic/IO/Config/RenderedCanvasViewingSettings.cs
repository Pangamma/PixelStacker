using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic.IO.Config
{
    class RenderedCanvasViewingSettings
    {
        /// <summary>
        /// Purely for aesthetic rendering. Assuming no block/material filter is set, any blocks with
        /// only 1 type of material will show up as layer 1 instead of on both layers. So... some 
        /// shadows will appear to give the image more depth.
        /// </summary>
        //public bool IsExtraShadowDepthEnabled
        //{
        //    get => _IsExtraShadowDepthEnabled && this.IsAdvancedModeEnabled;
        //    set => _IsExtraShadowDepthEnabled = value && this.IsAdvancedModeEnabled;
        //}
        //private bool _IsExtraShadowDepthEnabled = false;

    }
}
