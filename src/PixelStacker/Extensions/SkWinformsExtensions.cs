﻿using SkiaSharp;
using System;
using System.Drawing;

namespace PixelStacker.Extensions
{
    public static class SkWinformsExtensions
    {
        /// <summary>
        /// Tries to parse #FF00AA hex format to a SKColor
        /// </summary>
        /// <param name="needle"></param>
        /// <returns></returns>
        public static SKColor? ToSKColor(this string needle)
        {
            if (needle.StartsWith("#"))
            {
                try
                {
                    int R, G, B;
                    string needleTrim = needle.Trim();

                    if (needleTrim.Length == 7)
                    {
                        R = Convert.ToByte(needleTrim.Substring(1, 2), 16);
                        G = Convert.ToByte(needleTrim.Substring(3, 2), 16);
                        B = Convert.ToByte(needleTrim.Substring(5, 2), 16);
                    }
                    else if (needleTrim.Length == 4)
                    {
                        R = Convert.ToByte(needleTrim.Substring(1, 1) + needleTrim.Substring(1, 1), 16);
                        G = Convert.ToByte(needleTrim.Substring(2, 1) + needleTrim.Substring(2, 1), 16);
                        B = Convert.ToByte(needleTrim.Substring(3, 1) + needleTrim.Substring(3, 1), 16);
                    }
                    else
                    {
                        return null;
                    }

                    SKColor cNeedle = new SKColor((byte)R, (byte)G, (byte)B, (byte)255);
                    return cNeedle;
                }
                catch (Exception) { }
            }

            return null;
        }

        public static SKBitmap BitmapToSKBitmap(this Bitmap bitmap)
        {
            var info = new SKImageInfo(bitmap.Width, bitmap.Height);
            var skiaBitmap = new SKBitmap(info);

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    skiaBitmap.SetPixel(i, j, new SKColor(color.R, color.G, color.B, color.A));
                }
            }

            return skiaBitmap;
        }

        public static Bitmap SKBitmapToBitmap(this SKBitmap bitmap)
        {
            var info = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    SKColor color = bitmap.GetPixel(i, j);
                    info.SetPixel(i, j, Color.FromArgb((int)color.Alpha, (int)color.Red, (int)color.Green, (int)color.Blue));
                }
            }

            return info;
        }

        /// <summary>
        /// Copies the original image into a NEW image.
        /// If you are done with your original image, you should
        /// dispose it yourself.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <returns></returns>
        public static Bitmap Resize(this Bitmap bitmap, int w, int h)
        {
            Bitmap bm = new Bitmap(w, h);
            using Graphics g = Graphics.FromImage(bm);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(bitmap, 0, 0, w, h);
            return bm;
        }
    }
}
