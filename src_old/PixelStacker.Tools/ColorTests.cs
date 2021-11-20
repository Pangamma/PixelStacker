using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using PixelStacker.Logic.Extensions;
using System.Threading;
using System.Globalization;
using System.Linq;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.IO;
using PixelStacker.Logic;

namespace PixelStacker.Tools
{
    
    [TestClass]
    public class ColorTests
    {
        const int HUE_DIVISER = 24; //24; // 30 in each fragment, 12 total fragments. 
        const int SAT_FACTOR = 12; // 30 in each fragment, 12 total fragments. 

        public int ToInt255(float f)
        {
            return (int) (f * 255);
        }


        [TestMethod]
        public void BuildColorPalette()
        {
            List<Color> colorsFromMaterials = new List<Color>();
            bool isv = false;
            var mats = Materials.List;
            var mats2 = Materials.List.Where(x => x.Category == "Glass" || x.PixelStackerID == "AIR").ToList();
            for (int a = 0; a < mats.Count; a++)
            {
                var ca = mats[a].getAverageColor(isv);
                for (int b = 0; b < mats2.Count; b++)
                {
                    var cb = mats[b].getAverageColor(isv);
                    var cba = ca.OverlayColor(cb);
                    colorsFromMaterials.Add(cba);
                }
            }

            // H: grouping horizontally. Maybe 10 or 18 total groupings. or 24 / 15
            // S: Divide each column by the saturation. Maybe into 5 chunks. Exponential curve should be used...
            // V: light to dark linear

            var hsvList = new List<List<Color>>();
            #region obtain hsvList
            {
                var hueBuckets = new Dictionary<float, List<Color>>();
                foreach (var ci in colorsFromMaterials)
                {
                    var _h = ((int) ci.GetHue()) / HUE_DIVISER;

                    if (ci.GetSaturation() < 0.2)
                    {
                        _h = -100;
                        _h += ((int) (ci.GetBrightness() * 50));
                    }
                    if (ci.GetBrightness() < 0.2)
                    {
                        _h = -2;
                    }
                    else if (ci.GetBrightness() > 0.8)
                    {
                        _h = -1;
                    }

                    if (!hueBuckets.ContainsKey(_h))
                    {
                        hueBuckets[_h] = new List<Color>(125);
                    }

                    hueBuckets[_h].Add(ci);
                }

                hsvList = hueBuckets.OrderBy(x => x.Key).Select(x => x.Value).ToList();
            }

            #endregion
            var hsvSplitBySat = new List<List<Color>>();
            foreach(var hsvBucket in hsvList)
            {
                var testtt = hsvBucket.Select(x => x.GetSaturation()).ToList();
                var splitBySat = hsvBucket.GroupBy(x => ToInt255(x.GetSaturation()) / SAT_FACTOR)
                    .Select(g => g.ToList())
                    ;
                hsvSplitBySat.AddRange(splitBySat);

                int maxC = splitBySat.Max(x => x.Count);
                var bigL = splitBySat.First(x => x.Count == maxC);
                var maxSat = bigL.Max(b => b.GetSaturation());
                var minSat = bigL.Min(b => b.GetSaturation());
                var maxB = bigL.Max(b => b.GetBrightness());
                var minB = bigL.Min(b => b.GetBrightness());
                var maxH = bigL.Max(b => b.GetHue());
                var minH = bigL.Min(b => b.GetHue());
            }

            hsvList = hsvSplitBySat;
            int min = hsvList.Min(x => x.Count);
            int minBuck = 5;

            int n = 1000;
            while (n-- > 0 && min < minBuck)
            {
                hsvList = MergeBuckets(hsvList, minBuck);
                min = hsvList.Min(x => x.Count);
            }

            {
                int width = hsvList.Count;
                int height = hsvList.Max(hb => hb.Count);
                var bm = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                int x = 0; int y = 0;
                foreach (var hsvBucket in hsvList)
                {
                    var val = hsvBucket.OrderBy(v => v.GetBrightness()).Reverse().ToList();
                    foreach (var color in val)
                    {
                        bm.SetPixel(x, y, color);

                        y++;
                    }
                    x++; y = 0;
                }

                File.Delete(@"D:\git\PixelStacker\src\PixelStacker\bin\test.png");
                bm.Save(@"D:\git\PixelStacker\src\PixelStacker\bin\test.png", ImageFormat.Png);
            }
        }

        private List<KeyValuePair<float, List<Color>>> SplitBuckets(List<KeyValuePair<float, List<Color>>> buckets, int maxBucketSize)
        {
            int numSatBuckets = 4;
            List<KeyValuePair<float, List<Color>>> output = new List<KeyValuePair<float, List<Color>>>();
            var bucketList = buckets.OrderBy(x => x.Key)
                .SelectMany(x => x.Value.GroupBy(g => ((int) (g.GetSaturation() * 100)) / numSatBuckets))
                .Select(g => new KeyValuePair<float, List<Color>>(0.0F, g.ToList()))
                .ToList();

            for (int i = 0; i < bucketList.Count; i++)
            {
                var kvp = bucketList[i];
                output.Add(kvp);
            }

            return output;
        }

        private List<List<Color>> MergeBuckets(List<List<Color>> bucketList, int minBucketSize)
        {
            List<List<Color>> output = new List<List<Color>>();

            for (int i = 0; i < bucketList.Count; i += 2)
            {
                var kvpL = bucketList[i];
                if (i + 1 >= bucketList.Count)
                {
                    output.Add(kvpL);
                }
                else
                {
                    var kvpR = bucketList[i + 1];

                    if (kvpL.Count < minBucketSize || kvpR.Count < minBucketSize)
                    {
                        output.Add(kvpL.Concat(kvpR).ToList());
                    }
                    else
                    {
                        output.Add(kvpL);
                        output.Add(kvpR);
                    }
                }
            }

            return output;
        }
    }
}
