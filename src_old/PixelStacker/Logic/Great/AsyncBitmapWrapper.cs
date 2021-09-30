using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.Logic
{
    public class AsyncBitmapWrapper
    {
        public PixelFormat Format { get; }

        public int Width { get; }

        public int Height { get; }

        public IntPtr Buffer { get; }

        public int Stride { get; set; }

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
                    ImageLockMode.ReadOnly, bitmap.PixelFormat);

                Buffer = bitmapData.Scan0;
                Stride = bitmapData.Stride;
            } 
            finally
            {
                if (bitmapData != null)
                    bitmap.UnlockBits(bitmapData);
            }
        }

        public Bitmap ToBitmap()
        {
            return new Bitmap(Width, Height, Stride, Format, Buffer);
        }
    }
}
