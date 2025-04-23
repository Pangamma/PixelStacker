#if SKIA_SHARP
#pragma warning disable IDE1006 // Naming Styles
namespace PixelStacker.Resources
{
    using SkiaSharp;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class Textures {

        private static global::System.Resources.ResourceManager resourceMan;
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (resourceMan is null) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PixelStacker.Resources.Textures", typeof(Textures).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        private static SKBitmap _air = null;
        public static SKBitmap air
        {
            get
            {
                if (_air == null)
                    _air = SKBitmap.Decode((byte[])ResourceManager.GetObject("air"))
                    .Copy(SKColorType.Rgba8888);
                return _air;
            }
        }

        private static SKBitmap _barrier = null;
        public static SKBitmap barrier
        {
            get
            {
                if (_barrier == null)
                    _barrier = SKBitmap.Decode((byte[])ResourceManager.GetObject("barrier"))
                    .Copy(SKColorType.Rgba8888);
                return _barrier;
            }
        }

        private static SKBitmap _disabled = null;
        public static SKBitmap disabled
        {
            get
            {
                if (_disabled == null)
                    _disabled = SKBitmap.Decode((byte[])ResourceManager.GetObject("disabled"))
                    .Copy(SKColorType.Rgba8888);
                return _disabled;
            }
        }

        private static readonly string TextureFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Textures", "x16");

        private static Dictionary<string, SKBitmap> TextureCache = new();
        public static SKBitmap GetBitmap(string textureName, int rotationDegrees = 0)
        {
            string cacheKey = $"{rotationDegrees}--{textureName}";

            if (TextureCache.TryGetValue(cacheKey, out SKBitmap value))
            {
                return value;
            }

            string filePath = Path.Combine(TextureFolder, textureName + ".png");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Texture not found: {filePath}");

            using var stream = File.OpenRead(filePath);
            var original = SKBitmap.Decode(stream).Copy(SKColorType.Rgba8888);

            rotationDegrees = ((rotationDegrees % 360) + 360) % 360;

            if (rotationDegrees == 0)
            {
                TextureCache[cacheKey] = original;
                return original;
            }

            SKBitmap rotated;
            SKCanvas canvas;
            SKMatrix matrix;

            switch (rotationDegrees)
            {
                case 90:
                    rotated = new SKBitmap(original.Height, original.Width, SKColorType.Rgba8888, SKAlphaType.Premul);
                    matrix = SKMatrix.CreateRotationDegrees(90, 0, 0);
                    matrix = matrix.PostConcat(SKMatrix.CreateTranslation(rotated.Width, 0));
                    break;

                case 180:
                    rotated = new SKBitmap(original.Width, original.Height, SKColorType.Rgba8888, SKAlphaType.Premul);
                    matrix = SKMatrix.CreateRotationDegrees(180, 0, 0);
                    matrix = matrix.PostConcat(SKMatrix.CreateTranslation(rotated.Width, rotated.Height));
                    break;

                case 270:
                    rotated = new SKBitmap(original.Height, original.Width, SKColorType.Rgba8888, SKAlphaType.Premul);
                    matrix = SKMatrix.CreateRotationDegrees(270, 0, 0);
                    matrix = matrix.PostConcat(SKMatrix.CreateTranslation(0, rotated.Height));
                    break;

                default:
                    throw new ArgumentException("Rotation must be 0, 90, 180, or 270 degrees.");
            }

            canvas = new SKCanvas(rotated);
            canvas.Clear(SKColors.Transparent);
            canvas.SetMatrix(matrix);
            canvas.DrawBitmap(original, 0, 0);
            canvas.Flush();

            TextureCache[cacheKey] = rotated;
            return rotated;
        }
    }
}
#pragma warning restore IDE1006 // Naming Styles
#endif
