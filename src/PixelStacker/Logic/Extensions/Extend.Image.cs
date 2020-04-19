using FastBitmapLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Extensions
{
    /// <summary>
    /// The utility extender class.
    /// </summary>
    public static partial class Extend
    {
        #region bitmap
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
            int pixelSize = Image.GetPixelFormatSize(L.PixelFormat); // bits per pixel

            for (int i = 0; i < srcImageBytes.Length; i ++)
            {
                if (srcImageBytes[i] != dstImageBytes[i])
                {
                    return false;
                }
            }

            return true;
        }

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

            using (var fastbm = src.FastLock())
            {
                Color c = fastbm.GetPixel(x, y);
                return c;
            }
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

        /// <summary>
        /// Don't forget to dispose any unused images properly after calling this.
        /// Also CLONES the image instance to return a new image instance.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Bitmap To32bppBitmap(this Image src, int width, int height)
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

        /// <summary>
        /// Don't forget to dispose any unused images properly after calling this.
        /// Also CLONES the image instance to return a new image instance.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Bitmap To32bppBitmap(this Image src)
        {
            Bitmap output = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(output))
            {
                g.DrawImage(src, new Rectangle(0, 0, output.Width, output.Height));
            }
            return output;
        }

        public static Bitmap BlurBidirectional(this Bitmap src, int radiusH, int radiusV)
        {
            Bitmap output = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);

            using (var src32 = src.To32bppBitmap())
            {

                List<Color>[,] data = new List<Color>[src.Width, src.Height];

                using (var input = src32.FastLock())
                {
                    {
                        for (int xi = 0; xi < src32.Width; xi++)
                        {
                            for (int yi = 0; yi < src32.Height; yi++)
                            {
                                Color toInsert = input.GetPixel(xi, yi);
                                for (int x = Math.Max(0, xi - radiusH); x < Math.Min(src32.Width, xi + radiusH); x++)
                                {
                                    for (int y = Math.Max(0, yi - radiusV); y < Math.Min(src32.Height, y + radiusV); y++)
                                    {
                                        if (data[x, y] == null)
                                        {
                                            data[x, y] = new List<Color>();
                                        }

                                        data[x, y].Add(toInsert);
                                    }
                                }
                            }
                        }

                        for (int xi = 0; xi < src32.Width; xi++)
                        {
                            for (int yi = 0; yi < src32.Height; yi++)
                            {
                                var avg = data[xi, yi].AverageColors();
                                output.SetPixel(xi, yi, avg);
                            }
                        }
                    }
                }
            }

            return output;
        }
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
                ImageLockMode.ReadWrite,
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
                    TaskManager.SafeReport(100 * y / origImage.Height);
                }
            }


            //Get the bitmap data
            bitmapData = dstImage.LockBits(
                new Rectangle(0, 0, dstImage.Width, dstImage.Height),
                ImageLockMode.ReadWrite,
                dstImage.PixelFormat
            );

            //Copy the changed data into the bitmap again.
            System.Runtime.InteropServices.Marshal.Copy(imageBytes, 0, bitmapData.Scan0, imageBytes.Length);

            //Unlock the bitmap
            dstImage.UnlockBits(bitmapData);
        }


        /// <summary>
        /// Image MUST be 32bppARGB
        /// (int x, int y, Color cOrig, cDest) => { return newColorDest; }
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static void ToMergeStream(this Bitmap origImage, Bitmap dstImage, CancellationToken? worker, Func<int, int, Color, Color, Color> callback)
        {
            if (origImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            if (dstImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            //Get the bitmap data
            var srcData = origImage.LockBits(new Rectangle(0, 0, origImage.Width, origImage.Height), ImageLockMode.ReadWrite, origImage.PixelFormat);
            var dstData = dstImage.LockBits(new Rectangle(0, 0, dstImage.Width, dstImage.Height), ImageLockMode.ReadWrite, dstImage.PixelFormat);

            //Initialize an array for all the image data
            byte[] srcImageBytes = new byte[srcData.Stride * origImage.Height];
            byte[] dstImageBytes = new byte[dstData.Stride * dstImage.Height];

            //Copy the bitmap data to the local array
            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, srcImageBytes, 0, srcImageBytes.Length);
            System.Runtime.InteropServices.Marshal.Copy(dstData.Scan0, dstImageBytes, 0, dstImageBytes.Length);

            //Unlock the bitmap
            origImage.UnlockBits(srcData);
            dstImage.UnlockBits(dstData);

            //Find pixelsize
            int pixelSize = Image.GetPixelFormatSize(origImage.PixelFormat); // bits per pixel
            int bytesPerPixel = pixelSize / 8;
            int x = 0; int y = 0;
            var srcPixelData = new byte[bytesPerPixel];
            var dstPixelData = new byte[bytesPerPixel];
            for (int i = 0; i < srcImageBytes.Length; i += bytesPerPixel)
            {
                //Copy the bits into a local array
                Array.Copy(srcImageBytes, i, srcPixelData, 0, bytesPerPixel);
                Array.Copy(dstImageBytes, i, dstPixelData, 0, bytesPerPixel);

                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(srcPixelData);
                    Array.Reverse(dstPixelData);
                }

                //Get the color of a pixel
                // On a little-endian machine, the byte order is bb gg rr aa
                Color srcColor = Color.FromArgb(srcPixelData[3], srcPixelData[2], srcPixelData[1], srcPixelData[0]);
                Color dstColor = Color.FromArgb(dstPixelData[3], dstPixelData[2], dstPixelData[1], dstPixelData[0]);

                Color nColor = callback(x, y, srcColor, dstColor);
                dstPixelData[3] = nColor.A;
                dstPixelData[2] = nColor.R;
                dstPixelData[1] = nColor.G;
                dstPixelData[0] = nColor.B;
                Array.Copy(dstPixelData, 0, dstImageBytes, i, bytesPerPixel);

                x++;
                if (x > origImage.Width - 1)
                {
                    x = 0;
                    y++;
                    worker?.SafeThrowIfCancellationRequested();
                    TaskManager.SafeReport(100 * y / origImage.Height);
                }
            }


            //Get the bitmap data
            dstData = dstImage.LockBits(
                new Rectangle(0, 0, dstImage.Width, dstImage.Height),
                ImageLockMode.ReadWrite,
                dstImage.PixelFormat
            );

            //Copy the changed data into the bitmap again.
            System.Runtime.InteropServices.Marshal.Copy(dstImageBytes, 0, dstData.Scan0, dstImageBytes.Length);

            //Unlock the bitmap
            dstImage.UnlockBits(dstData);
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
                ImageLockMode.ReadWrite,
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
                    TaskManager.SafeReport(100 * y / origImage.Height);
                }
            }


            //Get the bitmap data
            bitmapData = origImage.LockBits(
                new Rectangle(0, 0, origImage.Width, origImage.Height),
                ImageLockMode.ReadWrite,
                origImage.PixelFormat
            );

            //Copy the changed data into the bitmap again.
            System.Runtime.InteropServices.Marshal.Copy(imageBytes, 0, bitmapData.Scan0, imageBytes.Length);

            //Unlock the bitmap
            origImage.UnlockBits(bitmapData);
        }


        /// <summary>
        /// Image MUST be 32bppARGB
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static void ToViewStream(this Bitmap origImage, CancellationToken? worker, Action<int, int, Color> callback)
        {
            if (origImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            //Get the bitmap data
            var bitmapData = origImage.LockBits(
                new Rectangle(0, 0, origImage.Width, origImage.Height),
                ImageLockMode.ReadWrite,
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
                callback(x, y, color);

                x++;
                if (x > origImage.Width - 1)
                {
                    x = 0;
                    y++;
                    worker?.SafeThrowIfCancellationRequested();
                    TaskManager.SafeReport(100 * y / origImage.Height);
                }
            }

            return;
        }



        /// <summary>
        /// Image MUST be 32bppARGB
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        [Obsolete("This thing might be causing some UI glitches. Double check before fully using it.")]
        public static void ToViewStreamParallel(this Bitmap origImage, CancellationToken? worker, Action<int, int, Color> callback)
        {
            if (origImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            //Get the bitmap data
            var bitmapData = origImage.LockBits(
                new Rectangle(0, 0, origImage.Width, origImage.Height),
                ImageLockMode.ReadWrite,
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
            //int x = 0; int y = 0;
            int origImageH = origImage.Height;
            int origImageW = origImage.Width;
            var pixelData = new byte[bytesPerPixel];

            int numPixelsTotal = imageBytes.Length / bytesPerPixel;
            int numPixelsFinished = 0;
            Parallel.For(0, numPixelsTotal, i =>
            //for (int i = 0; i < imageBytes.Length; i += bytesPerPixel)
            {
                //Copy the bits into a local array
                Array.Copy(imageBytes, i * bytesPerPixel, pixelData, 0, bytesPerPixel);

                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(pixelData);
                }

                //Get the color of a pixel
                // On a little-endian machine, the byte order is bb gg rr aa
                Color color = Color.FromArgb(pixelData[3], pixelData[2], pixelData[1], pixelData[0]);
                int x = i % origImageW;
                int y = i / origImageW;
                callback(x, y, color);

                if (x > origImageW - 1)
                {
                    numPixelsFinished++;
                    worker?.SafeThrowIfCancellationRequested();
                    TaskManager.SafeReport(100 * numPixelsFinished / numPixelsTotal);
                }
            });

            return;
        }
        #endregion

        #region | Palette methods |

        /// <summary>
        /// Gets the palette color count.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static Int32 GetPaletteColorCount(this Image image)
        {
            // checks whether a source image is valid
            if (image == null)
            {
                const String message = "Cannot assign a palette to a null image.";
                throw new ArgumentNullException(message);
            }

            // checks if the image has an indexed format
            if (!image.PixelFormat.IsIndexed())
            {
                String message = string.Format("Cannot retrieve a color count from a non-indexed image with pixel format '{0}'.", image.PixelFormat);
                throw new InvalidOperationException(message);
            }

            // returns the color count
            return image.Palette.Entries.Length;
        }

        /// <summary>
        /// Gets the palette of an indexed image.
        /// </summary>
        /// <param name="image">The source image.</param>
        public static List<Color> GetPalette(this Image image)
        {
            // checks whether a source image is valid
            if (image == null)
            {
                const String message = "Cannot assign a palette to a null image.";
                throw new ArgumentNullException(message);
            }

            // checks if the image has an indexed format
            if (!image.PixelFormat.IsIndexed())
            {
                String message = string.Format("Cannot retrieve a palette from a non-indexed image with pixel format '{0}'.", image.PixelFormat);
                throw new InvalidOperationException(message);
            }

            // retrieves and returns the palette
            return image.Palette.Entries.ToList();
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
                const String message = "Cannot assign a null palette.";
                throw new ArgumentNullException(message);
            }

            // checks whether a target image is valid
            if (image == null)
            {
                const String message = "Cannot assign a palette to a null image.";
                throw new ArgumentNullException(message);
            }

            // checks if the image has indexed format
            if (!image.PixelFormat.IsIndexed())
            {
                String message = string.Format("Cannot store a palette to a non-indexed image with pixel format '{0}'.", image.PixelFormat);
                throw new InvalidOperationException(message);
            }

            // retrieves a target image palette
            ColorPalette imagePalette = image.Palette;

            // checks if the palette can fit into the image palette
            if (palette.Count > imagePalette.Entries.Length)
            {
                String message = string.Format("Cannot store a palette with '{0}' colors intto an image palette where only '{1}' colors are allowed.", palette.Count, imagePalette.Entries.Length);
                throw new ArgumentOutOfRangeException(message);
            }

            // copies all color entries
            for (Int32 index = 0; index < palette.Count; index++)
            {
                imagePalette.Entries[index] = palette[index];
            }

            // assigns the palette to the target image
            image.Palette = imagePalette;
        }

        #endregion
    }
}