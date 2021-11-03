using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Extensions
{
    /// <summary>
    /// The utility extender class.
    /// </summary>
    public static partial class Extend
    {
        [Obsolete]
        public static Bitmap ToBitmap(this byte[] data)
        {
            using (var ms = new System.IO.MemoryStream(data))
            {
                var bm = new Bitmap(ms);
                return bm;
            }
        }

        [Obsolete]
        public static Bitmap To32bppBitmap(this byte[] data)
        {
            using (var ms = new System.IO.MemoryStream(data))
            {
                var bm = new Bitmap(ms);
                return bm;
            }
        }

        #region bitmap
        [Obsolete]
        public static bool AreEqual(this Bitmap L, Bitmap R)
        {
            if (L != null ^ R != null) return false;
            if (L.Width != R.Width) return false;
            if (R.Height != R.Height) return false;
            if (L.PixelFormat != R.PixelFormat) return false;

            //Get the bitmap data
            var srcData = L.LockBits(new Rectangle(0, 0, L.Width, L.Height), ImageLockMode.ReadOnly, L.PixelFormat);
            var dstData = R.LockBits(new Rectangle(0, 0, R.Width, R.Height), ImageLockMode.ReadOnly, R.PixelFormat);

            //Initialize an array for all the image data
            byte[] srcImageBytes = new byte[srcData.Stride * L.Height];
            byte[] dstImageBytes = new byte[dstData.Stride * R.Height];

            //Copy the bitmap data to the local array
            Marshal.Copy(srcData.Scan0, srcImageBytes, 0, srcImageBytes.Length);
            Marshal.Copy(dstData.Scan0, dstImageBytes, 0, dstImageBytes.Length);

            //Unlock the bitmap
            L.UnlockBits(srcData);
            R.UnlockBits(dstData);

            //Find pixelsize
            int _ = Image.GetPixelFormatSize(L.PixelFormat); // bits per pixel

            for (int i = 0; i < srcImageBytes.Length; i++)
            {
                if (srcImageBytes[i] != dstImageBytes[i])
                {
                    return false;
                }
            }

            return true;
        }


        [Obsolete]
        public static Color GetPixelSafely(this Bitmap src, int x, int y)
        {
            if (src == null) return Color.Transparent;
            if (x < 0 || y < 0 || x >= src.Width || y >= src.Height) return Color.Transparent;
            if (!CanReadPixel(src.PixelFormat)) return Color.Transparent;
            if ((src.PixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed)
            {
                Color c = src.GetPixel(x, y);
                return c;
            }

            //using (var fastbm = src.FastLock())
            //{
            //    Color c = fastbm.GetPixel(x, y);
            //    return c;
            //}
            return Color.Transparent;
        }

        /// <summary>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="normalize">If colors should be put into their buckets of 5 or 15 or or whatever.</param>
        /// <returns></returns>
        public static List<SKColor> GetColorsInImage(this SKBitmap src, int rgbBucketSize = 1)
        {
            List<SKColor> cs = new List<SKColor>();
            src.ToViewStream(null, (int x, int y, SKColor c) =>
            {
                cs.Add(rgbBucketSize == 1 ? c : c.Normalize(rgbBucketSize));
            });

            return cs;
        }

        /// <summary>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="normalize">If colors should be put into their buckets of 5 or 15 or or whatever.</param>
        /// <returns></returns>
        public static SKColor GetAverageColor(this SKBitmap src, int rgbBucketSize = 1)
        {
            long r = 0;
            long g = 0;
            long b = 0;
            long a = 0;
            long total = src.Width * src.Height;

            src.ToViewStream(null, (int x, int y, SKColor c) =>
            {
                Interlocked.Add(ref r, c.Red);
                Interlocked.Add(ref g, c.Green);
                Interlocked.Add(ref b, c.Blue);
                Interlocked.Add(ref a, c.Alpha);
            });

            r /= total;
            g /= total;
            b /= total;
            a /= total;

            if (a > 128)
            {
                a = 255;
            }

            SKColor rt = new SKColor((byte)r, (byte)g, (byte)b, (byte)a);
            return rgbBucketSize == 1 ? rt : rt.Normalize(rgbBucketSize);
        }

        private static bool CanReadPixel(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    return true;
                case PixelFormat.Format32bppArgb:
                    return true;
                default: return false;
            }
        }

        [Obsolete]
        /// <summary>
        /// Don't forget to dispose any unused images properly after calling this.
        /// Also CLONES the image instance to return a new image instance.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Bitmap To32bppBitmap(this Image src, int width, int height)
        {
            lock (src)
            {
                Bitmap output = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(output))
                {
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.DrawImage(src, new Rectangle(0, 0, output.Width, output.Height));
                }
                return output;
            }
        }

        [Obsolete]
        /// <summary>
        /// Don't forget to dispose any unused images properly after calling this.
        /// Also CLONES the image instance to return a new image instance.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Bitmap To32bppBitmap(this Image src)
        {
            lock (src)
            {
                Bitmap output = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(output))
                {
                    g.DrawImage(src, new Rectangle(0, 0, output.Width, output.Height));
                    return output;
                }
            }
        }

        [Obsolete]
        /// <summary>
        /// Image MUST be 32bppARGB
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static void ToCopyStream(this Bitmap origImage, Bitmap dstImage, CancellationToken? worker, Func<int, int, Color, Color> callback)
        {
            if (origImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            //Get the bitmap data
            var bitmapData = origImage.LockBits(
                new Rectangle(0, 0, origImage.Width, origImage.Height),
                ImageLockMode.ReadOnly,
                origImage.PixelFormat
            );

            //Initialize an array for all the image data
            byte[] imageBytes = new byte[bitmapData.Stride * origImage.Height];

            //Copy the bitmap data to the local array
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, imageBytes, 0, imageBytes.Length);

            //Unlock the bitmap
            origImage.UnlockBits(bitmapData);

            //Find pixelsize
            int pixelSize = Image.GetPixelFormatSize(origImage.PixelFormat); // bits per pixel
            int bytesPerPixel = pixelSize / 8;
            int x = 0; int y = 0;
            var pixelData = new byte[bytesPerPixel];
            for (int i = 0; i < imageBytes.Length; i += bytesPerPixel)
            {
                //Copy the bits into a local array
                Array.Copy(imageBytes, i, pixelData, 0, bytesPerPixel);

                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(pixelData);
                }

                //Get the color of a pixel
                // On a little-endian machine, the byte order is bb gg rr aa
                Color color = Color.FromArgb(pixelData[3], pixelData[2], pixelData[1], pixelData[0]);
                Color nColor = callback(x, y, color);
                pixelData[3] = nColor.A;
                pixelData[2] = nColor.R;
                pixelData[1] = nColor.G;
                pixelData[0] = nColor.B;
                Array.Copy(pixelData, 0, imageBytes, i, bytesPerPixel);

                x++;
                if (x > origImage.Width - 1)
                {
                    x = 0;
                    y++;
                    worker?.SafeThrowIfCancellationRequested();
                    if (worker != null)
                        ProgressX.Report(100 * y / origImage.Height);
                }
            }


            //Get the bitmap data
            bitmapData = dstImage.LockBits(
                new Rectangle(0, 0, dstImage.Width, dstImage.Height),
                ImageLockMode.WriteOnly,
                dstImage.PixelFormat
            );

            //Copy the changed data into the bitmap again.
            System.Runtime.InteropServices.Marshal.Copy(imageBytes, 0, bitmapData.Scan0, imageBytes.Length);

            //Unlock the bitmap
            dstImage.UnlockBits(bitmapData);
        }


        /// <summary>
        /// Returns a NEW image.
        /// Image MUST be 32bppARGB
        /// (int x, int y, Color cOrig, cDest) => { return newColorDest; }
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static SKBitmap ToMergeStream(this SKBitmap origImage, SKBitmap dstImage, CancellationToken? worker, Func<int, int, SKColor, SKColor, SKColor> callback)
        {
            if (origImage.ColorType != SKColorType.Rgba8888)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            if (dstImage.ColorType != SKColorType.Rgba8888)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            var srcImagePixels = origImage.Pixels;
            var dstImagePixels = dstImage.Pixels;

            if (srcImagePixels.Length != dstImagePixels.Length)
            {
                throw new ArgumentException("Image sizes must be the same.");
            }

            var outImage = new SKBitmap(origImage.Width, origImage.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var outImagePixels = outImage.Pixels;

            int w = origImage.Width;
            int h = origImage.Height;
            for (int i = 0; i < srcImagePixels.Length; ++i)
            {
                int x = i % w;
                int y = i / w;

                outImagePixels[i] = callback(x, y, srcImagePixels[i], dstImagePixels[i]);
                if (x == 0)
                {
                    worker?.SafeThrowIfCancellationRequested();
                    if (worker != null) ProgressX.Report(100 * y / h);
                }
            }

            // Do we really need this?
            outImage.Pixels = outImagePixels;
            return outImage;
        }

        /// <summary>
        /// Returns a NEW image.
        /// Image MUST be 32bppARGB
        /// (int x, int y, Color cOrig, cDest) => { return newColorDest; }
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static SKBitmap ToEditStream(this SKBitmap origImage, CancellationToken? worker, Func<int, int, SKColor, SKColor> callback)
        {
            if (origImage.ColorType != SKColorType.Rgba8888)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            var srcImagePixels = origImage.Pixels;

            var outImage = new SKBitmap(origImage.Width, origImage.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
            var outImagePixels = outImage.Pixels;

            int w = origImage.Width;
            int h = origImage.Height;
            for (int i = 0; i < srcImagePixels.Length; ++i)
            {
                int x = i % w;
                int y = i / w;

                outImagePixels[i] = callback(x, y, srcImagePixels[i]);
                if (x == 0)
                {
                    worker?.SafeThrowIfCancellationRequested();
                    if (worker != null) ProgressX.Report(100 * y / h);
                }
            }

            // Do we really need this?
            outImage.Pixels = outImagePixels;
            return outImage;
        }

        /// <summary>
        /// Image MUST be 32bppARGB
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static void ToEditStream(this Bitmap origImage, CancellationToken? worker, Func<int, int, Color, Color> callback)
        {
            if (origImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            //Get the bitmap data
            var bitmapData = origImage.LockBits(
                new Rectangle(0, 0, origImage.Width, origImage.Height),
                ImageLockMode.ReadOnly,
                origImage.PixelFormat
            );

            //Initialize an array for all the image data
            byte[] imageBytes = new byte[bitmapData.Stride * origImage.Height];

            //Copy the bitmap data to the local array
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, imageBytes, 0, imageBytes.Length);

            //Unlock the bitmap
            origImage.UnlockBits(bitmapData);

            //Find pixelsize
            int pixelSize = Image.GetPixelFormatSize(origImage.PixelFormat); // bits per pixel
            int bytesPerPixel = pixelSize / 8;
            int x = 0; int y = 0;
            var pixelData = new byte[bytesPerPixel];
            for (int i = 0; i < imageBytes.Length; i += bytesPerPixel)
            {
                //Copy the bits into a local array
                Array.Copy(imageBytes, i, pixelData, 0, bytesPerPixel);

                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(pixelData);
                }

                //Get the color of a pixel
                // On a little-endian machine, the byte order is bb gg rr aa
                Color color = Color.FromArgb(pixelData[3], pixelData[2], pixelData[1], pixelData[0]);
                Color nColor = callback(x, y, color);
                pixelData[3] = nColor.A;
                pixelData[2] = nColor.R;
                pixelData[1] = nColor.G;
                pixelData[0] = nColor.B;
                Array.Copy(pixelData, 0, imageBytes, i, bytesPerPixel);

                x++;
                if (x > origImage.Width - 1)
                {
                    x = 0;
                    y++;

                    worker?.SafeThrowIfCancellationRequested();
                    if (worker != null)
                        ProgressX.Report(100 * y / origImage.Height);
                }
            }


            //Get the bitmap data
            bitmapData = origImage.LockBits(
                new Rectangle(0, 0, origImage.Width, origImage.Height),
                ImageLockMode.WriteOnly,
                origImage.PixelFormat
            );

            //Copy the changed data into the bitmap again.
            System.Runtime.InteropServices.Marshal.Copy(imageBytes, 0, bitmapData.Scan0, imageBytes.Length);

            //Unlock the bitmap
            origImage.UnlockBits(bitmapData);
        }


        [Obsolete("", true)]
        /// <summary>
        /// Image MUST be 32bppARGB
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static void ToEditStreamParallel(this Bitmap origImage, CancellationToken? worker, Func<int, int, Color, Color> callback)
        {
            if (origImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            //Get the bitmap data
            var srcData = origImage.LockBits(
                new Rectangle(0, 0, origImage.Width, origImage.Height),
                ImageLockMode.ReadOnly,
                origImage.PixelFormat
            );

            //Initialize an array for all the image data
            byte[] imageBytes = new byte[srcData.Stride * origImage.Height];

            //Copy the bitmap data to the local array
            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, imageBytes, 0, imageBytes.Length);

            //Unlock the bitmap
            origImage.UnlockBits(srcData);

            int numYProcessed = 0;
            int bitsPerPixel = Image.GetPixelFormatSize(origImage.PixelFormat); // bits per pixel
            int bytesPerPixel = bitsPerPixel / 8;
            int heightInPixels = origImage.Height;
            int widthInPixels = origImage.Width;
            int widthInBytes = Math.Abs(srcData.Stride);
            var pixelData = new byte[bytesPerPixel];

            Parallel.For(0, heightInPixels, y =>
            {
                int bYOffset = (int)(y * widthInBytes);
                for (int x = 0; x < widthInPixels; x++)
                {
                    int bXOffset = bYOffset + (x * bytesPerPixel);
                    int a0, r0, g0, b0;

                    if (BitConverter.IsLittleEndian)
                    {
                        a0 = bXOffset + 3;
                        r0 = bXOffset + 2;
                        g0 = bXOffset + 1;
                        b0 = bXOffset;
                    }
                    else
                    {
                        a0 = bXOffset;
                        r0 = bXOffset + 1;
                        g0 = bXOffset + 2;
                        b0 = bXOffset + 3;
                    }

                    Color color = Color.FromArgb(imageBytes[a0], imageBytes[r0], imageBytes[g0], imageBytes[b0]);
                    Color nColor = callback(x, y, color);
                    imageBytes[a0] = nColor.A;
                    imageBytes[r0] = nColor.R;
                    imageBytes[g0] = nColor.G;
                    imageBytes[b0] = nColor.B;
                }

                worker?.SafeThrowIfCancellationRequested();
                Interlocked.Increment(ref numYProcessed);
                if (worker != null)
                    ProgressX.Report((int)(100 * ((float)numYProcessed / heightInPixels)));
            });

            //Get the bitmap data
            srcData = origImage.LockBits(
                new Rectangle(0, 0, origImage.Width, origImage.Height),
                ImageLockMode.WriteOnly,
                origImage.PixelFormat
            );

            //Copy the changed data into the bitmap again.
            System.Runtime.InteropServices.Marshal.Copy(imageBytes, 0, srcData.Scan0, imageBytes.Length);

            //Unlock the bitmap
            origImage.UnlockBits(srcData);
        }


        /// <summary>
        /// Image MUST be 32bppARGB
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static void ToViewStream(this SKBitmap origImage, CancellationToken? worker, Action<int, int, SKColor> callback)
        {

            if (origImage.ColorType != SKColorType.Rgba8888)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            SKColor[] bitmapData = origImage.Pixels;

            int oH = origImage.Height;
            int oW = origImage.Width;
            int oL = bitmapData.Length;

            for (int i = 0; i < oL; i ++)
            {
                //Get the color of a pixel
                // On a little-endian machine, the byte order is bb gg rr aa
                SKColor color = bitmapData[i];

                callback(i % oW, i / oW, color);
                if (worker != null)
                {
                    ProgressX.Report(100 * (i / oW) / oH);
                    worker?.SafeThrowIfCancellationRequested();
                }
            }

            return;
        }
        #endregion

        #region | Palette methods |

        /// <summary>
        /// Gets the palette color count.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static int GetPaletteColorCount(this Image image)
        {
            // checks whether a source image is valid
            if (image == null)
            {
                const string message = "Cannot assign a palette to a null image.";
                throw new ArgumentNullException(nameof(image), message);
            }

            // checks if the image has an indexed format
            if (!image.PixelFormat.IsIndexed())
            {
                string message = string.Format("Cannot retrieve a color count from a non-indexed image with pixel format '{0}'.", image.PixelFormat);
                throw new InvalidOperationException(message);
            }

            // returns the color count
            return image.Palette.Entries.Length;
        }


        /// <summary>
        /// Sets the palette of an indexed image.
        /// </summary>
        /// <param name="image">The target image.</param>
        /// <param name="palette">The palette.</param>
        public static void SetPalette(this Image image, List<Color> palette)
        {
            // checks whether a palette is valid
            if (palette == null)
            {
                const string message = "Cannot assign a null palette.";
                throw new ArgumentNullException(nameof(palette), message);
            }

            // checks whether a target image is valid
            if (image == null)
            {
                const string message = "Cannot assign a palette to a null image.";
                throw new ArgumentNullException(nameof(image), message);
            }

            // checks if the image has indexed format
            if (!image.PixelFormat.IsIndexed())
            {
                string message = string.Format("Cannot store a palette to a non-indexed image with pixel format '{0}'.", image.PixelFormat);
                throw new InvalidOperationException(message);
            }

            // retrieves a target image palette
            ColorPalette imagePalette = image.Palette;

            // checks if the palette can fit into the image palette
            if (palette.Count > imagePalette.Entries.Length)
            {
                string message = string.Format("Cannot store a palette with '{0}' colors intto an image palette where only '{1}' colors are allowed.", palette.Count, imagePalette.Entries.Length);
                throw new ArgumentOutOfRangeException(message);
            }

            // copies all color entries
            for (int index = 0; index < palette.Count; index++)
            {
                imagePalette.Entries[index] = palette[index];
            }

            // assigns the palette to the target image
            image.Palette = imagePalette;
        }

        #endregion
    }
}