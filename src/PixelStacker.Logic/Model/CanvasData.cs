using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using PixelStacker.Extensions;

namespace PixelStacker.Logic.Model
{
    public class CanvasData
    {
        private int[,] BlocksMap { get; set; } // X, Y
        private int Width => BlocksMap.GetLength(0);
        private int Height => BlocksMap.GetLength(1);

        private MaterialPalette Palette { get; }

        public CanvasData(MaterialPalette palette)
        {
            this.Palette = palette;
        }

        public CanvasData(MaterialPalette palette, int[,] blocksMap)
        {
            this.Palette = palette;
            this.BlocksMap = blocksMap;
        }

        public Material this[int x, int y, bool isTopLayer]
        {
            get
            {
#if DEBUG
                if (this.Width <= x || x < 0)
                    throw new IndexOutOfRangeException($"X must be between 0 and {this.Width - 1}. Given: {x}");
                if (this.Height <= y || y < 0)
                    throw new IndexOutOfRangeException($"Y must be between 0 and {this.Height - 1}. Given: {y}");
#endif
                var comboID = this.BlocksMap[x, y];
                var combo = this.Palette[comboID];
                var rt = isTopLayer ? combo.Top : combo.Bottom;
                return rt;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
#if DEBUG
                if (this.Width <= x || x < 0)
                    throw new IndexOutOfRangeException($"X must be between 0 and {this.Width - 1}. Given: {x}");
                if (this.Height <= y || y < 0)
                    throw new IndexOutOfRangeException($"Y must be between 0 and {this.Height - 1}. Given: {y}");
#endif
                var existingComboID = this.BlocksMap[x, y];
                var existingCombo = this.Palette[existingComboID];
                Material top = isTopLayer ? value : existingCombo.Top;
                Material bot = !isTopLayer ? value : existingCombo.Bottom;
                var nCombo = new MaterialCombination(bot, top);
                int newComboID = Palette[nCombo];
                BlocksMap[x, y] = newComboID;
            }
        }

        public MaterialCombination this[int x, int y]
        {
            get
            {
#if DEBUG
                if (this.Width <= x || x < 0)
                    throw new IndexOutOfRangeException($"X must be between 0 and {this.Width - 1}. Given: {x}");
                if (this.Height <= y || y < 0)
                    throw new IndexOutOfRangeException($"Y must be between 0 and {this.Height - 1}. Given: {y}");
#endif
                var comboID = this.BlocksMap[x, y];
                var combo = this.Palette[comboID];
                return combo;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
#if DEBUG
                if (this.Width <= x || x < 0)
                    throw new IndexOutOfRangeException($"X must be between 0 and {this.Width - 1}. Given: {x}");
                if (this.Height <= y || y < 0)
                    throw new IndexOutOfRangeException($"Y must be between 0 and {this.Height - 1}. Given: {y}");
#endif
                int comboID = Palette[value]; // materialCombo
                BlocksMap[x, y] = comboID;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="System.OperationCanceledException">If cancelled.</exception>
        /// <param name="worker"></param>
        /// <returns></returns>
        public Task<Bitmap> ToBitmapAsync(CancellationToken? worker) 
        {
            int mHeight = this.Height;
            int mWidth = this.Width;
            Bitmap bm = new Bitmap(mWidth, mHeight, PixelFormat.Format32bppArgb);

            for(int x = 0; x < this.Width; x++)
            {
                worker?.SafeThrowIfCancellationRequested();
                for (int y = 0; y < this.Height; y++)
                {
                    int argb = this.BlocksMap[x, y];
                    Color c = Color.FromArgb(argb);
                    bm.SetPixel(x, y, c);
                }
            }

            return Task.FromResult(bm);
        }

        public static Task<CanvasData> FromBitmapAsync(MaterialPalette p, Bitmap bm, CancellationToken? worker)
        {
            int mHeight = bm.Height;
            int mWidth = bm.Width;

            var canvas = new CanvasData(p);
            canvas.BlocksMap = new int[mWidth, mHeight];

            for (int x = 0; x < mWidth; x++)
            {
                worker?.SafeThrowIfCancellationRequested();
                for (int y = 0; y < mHeight; y++)
                {
                    canvas.BlocksMap[x, y] = bm.GetPixel(x, y).ToArgb();
                }
            }

            return Task.FromResult(canvas);
        }
    }
}
