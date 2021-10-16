using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace PixelStacker.IO.Image 
{
    /// <summary>
    /// Bitmap prevents you from working on the same image space concurrently for safety reasons.
    /// If you know what you are doing, you can bypass this safety constraint and work on the
    /// memory sections concurrently. Just don't allow any overlap.
    /// </summary>
    public class AsyncBitmapWrapper
    {
        public PixelFormat Format { get; }

        public int Width { get; }

        public int Height { get; }

        public IntPtr Buffer { get; }

        public int Stride { get; set; }

        /// <summary>
        /// Locks the bits of the bitmap along with its pointer, gets the references to these
        /// bits of data, then unlocks the bitmap.
        /// </summary>
        /// <param name="bitmap"></param>
        public AsyncBitmapWrapper(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            Format = bitmap.PixelFormat;
            Width = bitmap.Width;
            Height = bitmap.Height;

            BitmapData bitmapData = null;

            try
            {
                bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    ImageLockMode.ReadWrite, bitmap.PixelFormat);

                Buffer = bitmapData.Scan0;
                Stride = bitmapData.Stride;
            } 
            finally
            {
                if (bitmapData != null)
                    bitmap.UnlockBits(bitmapData);
            }
        }

        /// <summary>
        /// Returns a bitmap where the pointer is shared with all other bitmaps created
        /// from this AsyncBitmapWrapper. Do NOT dispose this proxy. Dispose the source
        /// bitmap instead once all other operations are complete.
        /// </summary>
        /// <returns></returns>
        public Bitmap ToBitmap()
        {
            return new Bitmap(Width, Height, Stride, Format, Buffer);
        }
    }
}
