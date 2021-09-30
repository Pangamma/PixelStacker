using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PixelStacker.Logic;
using PixelStacker.Logic.Extensions;

namespace PixelStacker.Tools
{
    [TestClass]
    public class TextureAnalyzer
    {

        [TestMethod]
        public void ModifyConstructors()
        {
            StringBuilder sb = new StringBuilder();
            int n = 1;
            int diff = 50;
            //Materials.List.Where(m => m.Category == "Glass").ToList().ForEach(m => m.Roughness = n++ * diff);
            //Materials.List.Where(m => m.Category == "Concrete").ToList().ForEach(m => m.Roughness = n++ * diff);
            //Materials.List.Where(m => m.Category == "Clay").ToList().ForEach(m => m.Roughness = n++ * diff);
            //Materials.List.Where(m => m.Category == "Powder").ToList().ForEach(m => m.Roughness = n++ * diff);
            //Materials.List.Where(m => m.Category == "Wool").ToList().ForEach(m => m.Roughness = n++ * diff);

            Materials.List.Select(x => new { S = x.toConstructorString(), x = x, CAT = x.Category })
                .GroupBy(x => x.CAT).ToList().ForEach(g =>
                {
                    g.ToList().ForEach(v => sb.AppendLine(v.S));
                    sb.AppendLine();
                });

            string output = sb.ToString();
            Console.WriteLine(output);
        }

        [TestMethod]
        public void ToTable()
        {
            StringBuilder sb = new StringBuilder();
            bool isv = true;




            var nodes = new List<TextureStats>();
            Materials.List.ForEach(m => {
                var avg = m.getAverageColor(isv);
                
                byte r = byte.MaxValue;
                byte g = byte.MaxValue;
                byte b = byte.MaxValue;

                byte R = byte.MinValue;
                byte G = byte.MinValue;
                byte B = byte.MinValue;


                float hue = float.MaxValue;
                float HUE = float.MinValue;

                float sat = float.MaxValue;
                float SAT = float.MinValue;

                float BRI = float.MinValue;
                float bri = float.MaxValue;

                double maxD = double.MinValue;
                double minD = double.MaxValue;

                using (var img = m.getImage(isv).To32bppBitmap()) {

                    img.ToViewStreamParallel(null, (int x, int y, Color c) => {
                        if (c.R < r) r = c.R;
                        if (c.R > R) R = c.R;
                        if (c.R < g) g = c.G;
                        if (c.R > G) G = c.G;
                        if (c.B < b) b = c.B;
                        if (c.B > B) B = c.B;

                        if (c.GetSaturation() < sat) sat = c.GetSaturation();
                        if (c.GetSaturation() > SAT) SAT = c.GetSaturation();

                        if (c.GetBrightness() < bri) bri = c.GetBrightness();
                        if (c.GetBrightness() > BRI) BRI = c.GetBrightness();

                        if (c.GetHue() < hue) hue = c.GetHue();
                        if (c.GetHue() > HUE) HUE = c.GetHue();
                    });
                }

                var node = new TextureStats()
                {
                    PixelStackerID = m.PixelStackerID,
                    MinR = r,
                    MaxR = R,
                    MinG = g,
                    MaxG = G,
                    MinB = b,
                    MaxB = B,
                    MinSat = sat,
                    MaxSat = SAT,
                    MinBri = bri,
                    MaxBri = BRI,
                    MinHue = hue,
                    MaxHue = HUE,
                };
                nodes.Add(node);
            });

            string header = string.Join(", ", typeof(TextureStats).GetProperties().ToList().Select(x => x.Name));
            sb.AppendLine(header);
            nodes.ForEach(n => {
                string line = string.Join(", ", n.GetType().GetProperties().ToList().Select(prop => prop.GetValue(n)));
                sb.AppendLine(line);
            });

            string content = sb.ToString();
            File.WriteAllText(@"D:\git\PixelStacker\src\PixelStacker\bin\textures.csv", content);
        }
    }

    class TextureStats
    {
        public string PixelStackerID { get; set; }
        public byte MinR { get; set; }
        public byte MaxR { get;  set; }
        public byte MinG { get; internal set; }
        public byte MaxG { get; internal set; }
        public byte MinB { get; internal set; }
        public byte MaxB { get; internal set; }
        public float MinSat { get; internal set; }
        public float MaxSat { get; internal set; }
        public float MinBri { get; internal set; }
        public float MaxBri { get; internal set; }
        public float MinHue { get; internal set; }
        public float MaxHue { get; internal set; }
    }
}
