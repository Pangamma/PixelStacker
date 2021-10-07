using FastBitmapLib;
using PixelStacker.Logic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
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

        /// <summary>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="normalize">If colors should be put into their buckets of 5 or 15 or or whatever.</param>
        /// <returns></returns>
        public static List<Color> GetColorsInImage(this Bitmap src, bool normalize = true)
        {
            List<Color> cs = new List<Color>();
            src.ToViewStream(null, (int x, int y, Color c) =>
            {
                cs.Add(normalize ? c.Normalize() : c);
            });

            return cs;
        }

        /// <summary>
        /// </summary>
        /// <param name="src"></param>
        /// <param name="normalize">If colors should be put into their buckets of 5 or 15 or or whatever.</param>
        /// <returns></returns>
        public static Color GetAverageColor(this Bitmap src, bool normalize = true)
        {
            long r = 0;
            long g = 0;
            long b = 0;
            long a = 0;
            long total = src.Width * src.Height;

            src.ToViewStreamParallel(null, (int x, int y, Color c) =>
            {
                Interlocked.Add(ref r, c.R);
                Interlocked.Add(ref g, c.G);
                Interlocked.Add(ref b, c.B);
                Interlocked.Add(ref a, c.A);
            });

            r /= total;
            g /= total;
            b /= total;
            a /= total;

            if (a > 128)
            {
                a = 255;
            }

            Color rt = Color.FromArgb((int)a, (int)r, (int)g, (int)b);
            return normalize ? rt.Normalize() : rt;
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

        [Obsolete("Fix this for concurrency, and also make it so it outputs a patched image instead.", false)]
        /// <summary>
        /// Image MUST be 32bppARGB
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static void ToMergeStreamParallel(this Bitmap srcImage, Bitmap dstImage, CancellationToken? worker, Func<int, int, Color, Color, Color> callback)
        {
            if (srcImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("src PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            if (dstImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("dst PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            if (dstImage.Height != srcImage.Height || dstImage.Width != srcImage.Width)
            {
                throw new ArgumentException("Image dimensions do not match.");
            }

            //Get the bitmap data
            var srcData = srcImage.LockBits(new Rectangle(0, 0, srcImage.Width, srcImage.Height), ImageLockMode.ReadOnly, srcImage.PixelFormat);
            var dstData = dstImage.LockBits(new Rectangle(0, 0, dstImage.Width, dstImage.Height), ImageLockMode.ReadOnly, dstImage.PixelFormat);

            var srcBytes = new byte[srcData.Stride * srcImage.Height];
            var dstBytes = new byte[dstData.Stride * dstImage.Height];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, srcBytes, 0, srcBytes.Length);
            System.Runtime.InteropServices.Marshal.Copy(dstData.Scan0, dstBytes, 0, dstBytes.Length);

            srcImage.UnlockBits(srcData);
            dstImage.UnlockBits(dstData);

            int numYProcessed = 0;
            int bitsPerPixel = Image.GetPixelFormatSize(srcImage.PixelFormat); // bits per pixel
            int bytesPerPixel = bitsPerPixel / 8;
            int heightInPixels = srcImage.Height;
            int widthInPixels = srcImage.Width;
            int widthInBytes = Math.Abs(srcData.Stride);


            Parallel.For(0, heightInPixels, y =>
            {
                int bYOffset = (int) (y * widthInBytes);
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

                    Color srcColor = Color.FromArgb(srcBytes[a0], srcBytes[r0], srcBytes[g0], srcBytes[b0]);
                    Color dstColor = Color.FromArgb(dstBytes[a0], dstBytes[r0], dstBytes[g0], dstBytes[b0]);

                    Color nColor = callback(x / bytesPerPixel, y, srcColor, dstColor);
                    dstBytes[a0] = nColor.A;
                    dstBytes[r0] = nColor.R;
                    dstBytes[g0] = nColor.G;
                    dstBytes[b0] = nColor.B;
                }

                worker?.SafeThrowIfCancellationRequested();
                Interlocked.Increment(ref numYProcessed);
                ProgressX.Report((int) (100 * ((float) numYProcessed / heightInPixels)));
            });

            //Get the bitmap data
            dstData = dstImage.LockBits(
                new Rectangle(0, 0, srcImage.Width, srcImage.Height),
                ImageLockMode.WriteOnly,
                srcImage.PixelFormat
            );

            //Copy the changed data into the bitmap again.
            Marshal.Copy(dstBytes, 0, dstData.Scan0, dstBytes.Length);

            //Unlock the bitmap
            dstImage.UnlockBits(dstData);
            ProgressX.Report(100);
        }


        /// <summary>
        /// Returns a NEW image.
        /// Image MUST be 32bppARGB
        /// (int x, int y, Color cOrig, cDest) => { return newColorDest; }
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static Bitmap ToMergeStream(this Bitmap origImage, Bitmap dstImage, CancellationToken? worker, Func<int, int, Color, Color, Color> callback)
        {
            if (origImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            if (dstImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb.");
            }

            BitmapData srcData;
            BitmapData dstData;
            byte[] srcImageBytes;
            byte[] dstImageBytes;

            byte[] outImageBytes;
            var outImage = new Bitmap(origImage.Width, origImage.Height, PixelFormat.Format32bppArgb);

            lock (origImage)
            {
                lock (dstImage)
                {
                    //Get the bitmap data
                    srcData = origImage.LockBits(new Rectangle(0, 0, origImage.Width, origImage.Height), ImageLockMode.ReadOnly, origImage.PixelFormat);
                    dstData = dstImage.LockBits(new Rectangle(0, 0, dstImage.Width, dstImage.Height), ImageLockMode.ReadOnly, dstImage.PixelFormat);
                    //Initialize an array for all the image data
                    srcImageBytes = new byte[srcData.Stride * origImage.Height];
                    dstImageBytes = new byte[dstData.Stride * dstImage.Height];
                    outImageBytes = new byte[dstData.Stride * dstImage.Height];

                    //Copy the bitmap data to the local array
                    System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, srcImageBytes, 0, srcImageBytes.Length);
                    System.Runtime.InteropServices.Marshal.Copy(dstData.Scan0, dstImageBytes, 0, dstImageBytes.Length);

                    //Unlock the bitmap
                    origImage.UnlockBits(srcData);
                    dstImage.UnlockBits(dstData);
                }
            }

            //Find pixelsize
            int pixelSize = Image.GetPixelFormatSize(origImage.PixelFormat); // bits per pixel
            int bytesPerPixel = pixelSize / 8;
            int x = 0; int y = 0;
            var srcPixelData = new byte[bytesPerPixel];
            var outPixelData = new byte[bytesPerPixel];
            for (int i = 0; i < srcImageBytes.Length; i += bytesPerPixel)
            {
                //Copy the bits into a local array
                Array.Copy(srcImageBytes, i, srcPixelData, 0, bytesPerPixel);
                Array.Copy(dstImageBytes, i, outPixelData, 0, bytesPerPixel);

                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(srcPixelData);
                    Array.Reverse(outPixelData);
                }

                //Get the color of a pixel
                // On a little-endian machine, the byte order is bb gg rr aa
                Color srcColor = Color.FromArgb(srcPixelData[3], srcPixelData[2], srcPixelData[1], srcPixelData[0]);
                Color dstColor = Color.FromArgb(outPixelData[3], outPixelData[2], outPixelData[1], outPixelData[0]);

                Color nColor = callback(x, y, srcColor, dstColor);
                outPixelData[3] = nColor.A;
                outPixelData[2] = nColor.R;
                outPixelData[1] = nColor.G;
                outPixelData[0] = nColor.B;
                Array.Copy(outPixelData, 0, outImageBytes, i, bytesPerPixel);

                x++;
                if (x > origImage.Width - 1)
                {
                    x = 0;
                    y++;
                    worker?.SafeThrowIfCancellationRequested();
                    ProgressX.Report(100 * y / origImage.Height);
                }
            }


            //Get the bitmap data
            BitmapData outData = outImage.LockBits(
                new Rectangle(0, 0, outImage.Width, outImage.Height),
                ImageLockMode.WriteOnly,
                outImage.PixelFormat
            );

            //Copy the changed data into the bitmap again.
            System.Runtime.InteropServices.Marshal.Copy(outImageBytes, 0, outData.Scan0, outImageBytes.Length);

            //Unlock the bitmap
            outImage.UnlockBits(outData);

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
                int bYOffset = (int) (y * widthInBytes);
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
                ProgressX.Report((int) (100 * ((float) numYProcessed / heightInPixels)));
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
        public static void ToViewStream(this Bitmap origImage, CancellationToken? worker, Action<int, int, Color> callback)
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
                callback(x, y, color);

                x++;
                if (x > origImage.Width - 1)
                {
                    x = 0;
                    y++;
                    worker?.SafeThrowIfCancellationRequested();
                    ProgressX.Report(100 * y / origImage.Height);
                }
            }

            return;
        }

        /// <summary>
        /// Image SHOULD be 32bppARGB
        /// </summary>
        /// <param name="origImage"></param>
        /// <returns></returns>
        public static void ToViewStreamParallel(this Bitmap origImage, CancellationToken? worker, Action<int, int, Color> callback)
        {
            if (origImage.PixelFormat != PixelFormat.Format32bppArgb)
            {
                if (!CanReadPixel(origImage.PixelFormat) && (origImage.PixelFormat & PixelFormat.Indexed) != PixelFormat.Indexed)
                {
                    throw new ArgumentException("PixelFormat MUST be PixelFormat.Format32bppArgb, or else format must be indexed.");
                }

                for (int x = 0; x < origImage.Width; x++)
                {
                    for (int y = 0; y < origImage.Height; y++)
                    {
                        Color c = origImage.GetPixel(x, y);
                        callback(x, y, c);
                    }
                }

                return;
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

            Parallel.For(0, heightInPixels, y =>
            {
                int bYOffset = (int) (y * widthInBytes);
                for (int x = 0; x < widthInPixels; x++)
                {
                    int bXOffset = bYOffset + (x * bytesPerPixel);

                    Color color = BitConverter.IsLittleEndian
                    ? color = Color.FromArgb(imageBytes[bXOffset + 3], imageBytes[bXOffset + 2], imageBytes[bXOffset + 1], imageBytes[bXOffset])
                    : color = Color.FromArgb(imageBytes[bXOffset], imageBytes[bXOffset + 1], imageBytes[bXOffset + 2], imageBytes[bXOffset+3]);

                    callback(x, y, color);
                }

                worker?.SafeThrowIfCancellationRequested();
                Interlocked.Increment(ref numYProcessed);
                ProgressX.Report((int) (100 * ((float) numYProcessed / heightInPixels)));
            });
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