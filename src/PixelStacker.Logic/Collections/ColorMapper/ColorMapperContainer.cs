using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixelStacker.Logic.Collections.ColorMapper.DistanceFormulas;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.Model;

namespace PixelStacker.Logic.Collections.ColorMapper
{
    public static class ColorMapperContainer
    {
        private static object Padlock = new { };
        private static Dictionary<string, IColorMapper> MapperCache = new Dictionary<string, IColorMapper>();

        public static IColorMapper GetColorMapper(
            bool isSideView, bool isMultiLayer, 
            TextureMatchingStrategy textureMatchingStrategy,
            ColorDistanceFormulaType colorDistanceFormula)
        {
            string cacheKey = string.Format("{0}:{1}:{2}:{3}", 
                isSideView ? "1" : "0", 
                isMultiLayer ? "1" : "0",
                textureMatchingStrategy.ToString(),
                colorDistanceFormula.ToString()
            );

            if (MapperCache.TryGetValue(cacheKey, out var value))
            {
                return value;
            }

            lock (Padlock)
            {
                if (MapperCache.TryGetValue(cacheKey, out var value2))
                {
                    return value2;
                }

                var mapper = CreateColorMapper(textureMatchingStrategy, colorDistanceFormula);
                var palette = MaterialPalette.FromResx();
                var opts = new StaticJsonOptionsProvider().Load();
                var combosAvailable = palette.ToValidCombinationList(opts);
                if (!isMultiLayer) combosAvailable = combosAvailable.Where(x => !x.IsMultiLayer && x.Bottom.CanBeUsedAsBottomLayer).ToList();
                mapper.SetSeedData(combosAvailable, palette, isSideView);
                MapperCache[cacheKey] = mapper;
                return mapper;
            }
        }

        public static IColorMapper CreateColorMapper(
            TextureMatchingStrategy textureMatchingStrategy,
            ColorDistanceFormulaType colorDistanceFormula)
        {
            IColorDistanceFormula formula = colorDistanceFormula switch
            {
                ColorDistanceFormulaType.Hsl => new HslDistanceFormula(),
                ColorDistanceFormulaType.Srgb => new SrgbDistanceFormula(),
                ColorDistanceFormulaType.Rgb => new RgbDistanceFormula(),
                ColorDistanceFormulaType.RgbWithHue => new RgbWithHueDistanceFormula(),
                _ => throw new NotSupportedException(),
            };


            IColorMapper cm = new KdTreeColorMapper(formula, textureMatchingStrategy);
            return cm;
        }
    }
}
