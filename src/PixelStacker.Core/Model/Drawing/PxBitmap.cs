using System.Collections;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PixelStacker.Core.Model.Drawing
{
    public enum ImageLockMode
    {
        ReadOnly,
        WriteOnly,
        ReadWrite
    }

    public class PxBitmap : IEnumerable<XYData<PxColor>>, IDisposable
    {
        public int Width => Data.GetLength(0);
        public int Height => Data.GetLength(1);
        private PxColor[,] Data { get; set; }

        public PxColor GetAverageColor(int rgbFragmentSize)
        {
            var c = AverageColor.Value.Normalize(rgbFragmentSize);
            return c;
        }

        public Lazy<PxColor> AverageColor => new Lazy<PxColor>(() =>
        {
            long r = 0;
            long g = 0;
            long b = 0;
            long a = 0;
            long total = Width * Height;

            foreach (var kvp in Data)
            {
                r += kvp.R;
                g += kvp.G;
                b += kvp.B;
                a += kvp.A;
            }

            r /= total;
            g /= total;
            b /= total;
            a /= total;

            if (a > 128)
            {
                a = 255;
            }

            return PxColor.FromArgb((int)a, (int)r, (int)g, (int)b);
        });

        public PixelFormat PixelFormat => PixelFormat.Format32bppArgb;

        public static PxBitmap FromBytes(byte[] bmData)
        {
            SixLabors.ImageSharp.Image<Rgba32> data
                = SixLabors.ImageSharp.Image.Load(bmData);
            PxBitmap pixels = new PxBitmap(new PxColor[data.Width, data.Height]);

            for (int x = 0; x < data.Width; x++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    var pixel = data[x, y];
                    pixels[x, y] = PxColor.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
                }
            }

            return pixels;
        }

        internal static PxBitmap FromStream(Stream baseStream)
        {
            var bs = new byte[baseStream.Length];
            baseStream.Read(bs);
            return FromBytes(bs);
        }

        internal static PxBitmap FromSharpImage(Image<Rgba32> sharpImage)
        {
            var image = new PxBitmap(sharpImage.Width, sharpImage.Height);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var pixel = sharpImage[x, y];
                    image[x, y] = PxColor.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
                }
            }

            return image;
        }

        public Image<Rgba32> ToSharpImage()
        {
            var image = new Image<Rgba32>(this.Width, this.Height);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var pixel = this[x, y];
                    image[x, y] = new Rgba32(pixel.R, pixel.G, pixel.B, pixel.A);
                }
            }

            return image;
        }

        public byte[] ToBytes()
        {
            using var ms = new MemoryStream();
            this.ToSharpImage().SaveAsPng(ms);
            if (ms.CanSeek) ms.Seek(0, SeekOrigin.Begin);
            var rt = ms.ToArray();
            return rt;
        }

        internal void SetPalette(List<PxColor> value)
        {
            var img = Image.Load(new byte[0]);
            
            throw new NotImplementedException();
        }

        public PxBitmap(int w, int h)
        {
            Data = new PxColor[w, h];
        }

        public PxBitmap(PxBitmap original, int w, int h)
        {
            Data = new PxColor[w, h];
        }

        public PxBitmap(PxColor[,] data)
        {
            Data = data;
        }

        #region DATA ITERATION
        public ref PxColor this[int x, int y] => ref Data[x, y];
        public IEnumerator<XYData<PxColor>> GetEnumerator()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    yield return new XYData<PxColor>(x, y, this[x, y]);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    yield return new XYData<PxColor>(x, y, this[x, y]);
                }
            }
        }

        public PxBitmap ToMergeStream(PxBitmap other, CancellationToken? worker, Func<int, int, PxColor, PxColor, PxColor> callback)
        {
            if (other.Width != Width || Height != other.Height)
            {
                throw new ArgumentException(nameof(other), "Dimensions of the two bitmaps must match!");
            }

            PxBitmap result = new PxBitmap(Width, Height);

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; y++)
                {
                    var cA = this[x, y];
                    var cB = other[x, y];
                    result[x, y] = callback.Invoke(x, y, cA, cB);
                }
            }

            return result;
        }

        internal PxBitmap ToEditStream(CancellationToken? worker, Func<int, int, PxColor, PxColor> p)
        {
            PxBitmap result = new PxBitmap(Width, Height);

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; y++)
                {
                    var cA = this[x, y];
                    result[x, y] = p.Invoke(x, y, cA);
                }
            }

            return result;
        }

        public void ToViewStream(CancellationToken? worker, Action<int, int, PxColor> callback)
        {
            foreach (var item in this)
            {
                callback.Invoke(item.X, item.Y, item.Data);
            }
        }

        public PxBitmap To32bppBitmap()
        {
            System.Diagnostics.Debug.WriteLine("To32bppBitmap called");
            return this;
        }


        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Data = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion DATA ITERATION
    }
}
