using FastBitmapLib;
using PixelStacker.UI;
using SimplePaletteQuantizer.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public static class ImageExtensions
    {
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

            using(var fastbm = src.FastLock())
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
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
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
    }
}
