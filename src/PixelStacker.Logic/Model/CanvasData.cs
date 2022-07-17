using PixelStacker.Logic.Extensions;
using PixelStacker.Logic.Utilities;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PixelStacker.Logic.Model
{
    public class CanvasData : IEnumerable<CanvasIteratorData>
    {
        private int[,] BlocksMap { get; set; } // X, Y
        public int Width { get; }
        public int Height { get; }

        private MaterialPalette Palette { get; }

        public CanvasData(MaterialPalette palette, int[,] blocksMap)
        {
            this.Palette = palette;
            this.BlocksMap = blocksMap;
            this.Width = blocksMap.GetLength(0);
            this.Height = blocksMap.GetLength(1);
        }

        [Obsolete("Super dangerous. Be sure to check your inputs before using.", false)]
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public void SetDirectly(int x, int y, int PaletteID)
        {
            BlocksMap[x, y] = PaletteID;
        }

        public bool IsInRange(int x, int y)
        {
            if (this.Width <= x || x < 0) return false;
            if (this.Height <= y || y < 0) return false;
            return true;
        }

        public Material this[int x, int y, bool isTopLayer]
        {
            get
            {
#if FAIL_FAST
                if (this.Width <= x || x < 0)
                    throw new IndexOutOfRangeException($"X must be between 0 and {this.Width - 1}. Given: {x}");
                if (this.Height <= y || y < 0)
                    throw new IndexOutOfRangeException($"Y must be between 0 and {this.Height - 1}. Given: {y}");
#else
                if (this.Width <= x || x < 0)
                    return Materials.Air;
                if (this.Height <= y || y < 0)
                    return Materials.Air;
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
#if FAIL_FAST
                if (this.Width <= x || x < 0)
                    throw new IndexOutOfRangeException($"X must be between 0 and {this.Width - 1}. Given: {x}");
                if (this.Height <= y || y < 0)
                    throw new IndexOutOfRangeException($"Y must be between 0 and {this.Height - 1}. Given: {y}");
#endif
                var existingComboID = this.BlocksMap[x, y];
                var existingCombo = this.Palette[existingComboID];
                Material top = isTopLayer ? value : existingCombo.Top;
                Material bot = !isTopLayer ? value : existingCombo.Bottom;
#pragma warning disable CS0618 // Type or member can cause mem leaks if images are loaded and not properly disposed of.
                var nCombo = new MaterialCombination(bot, top);
                int newComboID = Palette[nCombo];
                BlocksMap[x, y] = newComboID;
#pragma warning restore CS0618 // Type or member can cause mem leaks if images are loaded.
            }
        }

        public MaterialCombination this[int x, int y]
        {
            get
            {
#if FAIL_FAST
                if (this.Width <= x || x < 0)
                    throw new IndexOutOfRangeException($"X must be between 0 and {this.Width - 1}. Given: {x}");
                if (this.Height <= y || y < 0)
                    throw new IndexOutOfRangeException($"Y must be between 0 and {this.Height - 1}. Given: {y}");
#else
                if (this.Width <= x || x < 0)
                    return this.Palette[IO.Config.Constants.MaterialCombinationIDForAir];
                if (this.Height <= y || y < 0)
                    return this.Palette[IO.Config.Constants.MaterialCombinationIDForAir];
#endif

                var comboID = this.BlocksMap[x, y];
                var combo = this.Palette[comboID];
                return combo;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
#if FAIL_FAST
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
        public Task<SKBitmap> ToBitmapAsync(CancellationToken? worker)
        {
            int mHeight = this.Height;
            int mWidth = this.Width;
            SKBitmap bm = new SKBitmap(mWidth, mHeight, SKColorType.Rgba8888, SKAlphaType.Premul);
            var canvas = new SKCanvas(bm);
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    worker?.SafeThrowIfCancellationRequested();
                    int argb = this.BlocksMap[x, y];
                    uint arg = (uint)argb;
                    SKColor c = ColorSerializer.FromPaletteID(arg);
                    //bm.SetPixel(x, y, c);
                    using var paint = new SKPaint() { Color = c, BlendMode = SKBlendMode.Src };
                    canvas.DrawPoint(x, y, paint);
                }
            }

            return Task.FromResult(bm);
        }

        public static Task<CanvasData> FromBitmapAsync(MaterialPalette p, SKBitmap bm, CancellationToken? worker)
        {
            int mHeight = bm.Height;
            int mWidth = bm.Width;

            var canvas = new CanvasData(p, new int[mWidth, mHeight]);

            for (int y = 0; y < mHeight; y++)
            {
                worker?.SafeThrowIfCancellationRequested();
                for (int x = 0; x < mWidth; x++)
                {
                    int argb = (int)(ColorSerializer.ToPaletteID(bm.GetPixel(x, y)));
                    canvas.BlocksMap[x, y] = argb;
                }
            }

            return Task.FromResult(canvas);
        }

        public IEnumerator<CanvasIteratorData> GetEnumerator()
        {
            int w = Width;
            int h = Height;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    yield return new CanvasIteratorData
                    {
                        X = x,
                        Y = y,
                        PaletteID = BlocksMap[x, y]
                    };
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            int w = Width;
            int h = Height;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    yield return new CanvasIteratorData
                    {
                        X = x,
                        Y = y,
                        PaletteID = BlocksMap[x, y]
                    };
                }
            }
        }
    }

    public class CanvasIteratorData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int PaletteID { get; set; }
    }
}
