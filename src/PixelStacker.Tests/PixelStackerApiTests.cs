using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic.API;
using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.Formatters;
using PixelStacker.Logic.Model;
using PixelStacker.Resources;
using SixLabors.ImageSharp.ColorSpaces;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PixelStacker.Tests
{
    [TestClass]
    public class PixelStackerApiTests
    {
        [TestMethod]
        public async Task RunThroughIt()
        {
            var api = new PixelStackerApi();
            var result = await api.RenderFromSkBitmap(DevResources.hyper_dimension, new RenderRequest()
            {
                EnableDithering = false,
                EnableShadows = true,
                Format = ExportFormat.PixelStackerProject,
                IsMultiLayer = true,
                IsSideView = false,
            });

            Assert.IsTrue(result.Data.Length > 0);
        }
    }
}
