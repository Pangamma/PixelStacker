using Newtonsoft.Json;
using PixelStacker.Logic.IO.Config;
using PixelStacker.Logic.IO.JsonConverters;
using PixelStacker.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PixelStacker.Logic.Model
{
    /// <summary>
    /// Should contain a list of all material combinations available for use, and then a way to index those back to a simplified ID that can be used for serialization.
    /// </summary>
    [JsonConverter(typeof(MaterialPaletteJsonConverter))]
    public class MaterialPalette
    {
        [JsonIgnore]
        internal Dictionary<int, MaterialCombination> FromPaletteID { get; private set; } = new Dictionary<int, MaterialCombination>();

        [JsonIgnore]
        protected Dictionary<MaterialCombination, int> ToPaletteID { get; private set; } = new Dictionary<MaterialCombination, int>();


        public int Count => ToPaletteID.Count;

        public static MaterialCombination Air => MaterialPalette.FromResx()[Constants.MaterialCombinationIDForAir];


        public int? GetPaletteIDByMaterials(Material bottom, Material top)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var key = new MaterialCombination(bottom, top);
#pragma warning restore CS0618 // Type or member is obsolete
            if (ToPaletteID.TryGetValue(key, out int val))
            {
                return val;
            }

            return null;
        }

        public MaterialCombination GetMaterialCombinationByMaterials(Material bottom, Material top)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var key = new MaterialCombination(bottom, top);
#pragma warning restore CS0618 // Type or member is obsolete
            if (ToPaletteID.TryGetValue(key, out int val))
            {
                return FromPaletteID[val];
            }

            return null;
        }

        /// <summary>
        /// We WILL throw errors here to ensure the program never passes with invalid data.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public MaterialCombination this[int index]
        {
            get
            {
                if (FromPaletteID.TryGetValue(index, out MaterialCombination val))
                {
                    return val;
                }

                throw new KeyNotFoundException($"No material combination exists for index {index}.");
            }
        }

        public bool ContainsKey(MaterialCombination i) => ToPaletteID.ContainsKey(i);
        public bool ContainsKey(int i) => FromPaletteID.ContainsKey(i);

        /// <summary>
        /// We WILL throw errors here to ensure the program never passes with invalid data.
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <returns></returns>
        public int this[MaterialCombination index]
        {
            get
            {
                if (ToPaletteID.TryGetValue(index, out int val))
                {
                    return val;
                }

                throw new KeyNotFoundException($"No index assigned for material combination {index}.");
            }
        }

        /// <summary>
        /// Returns false if combo was already added to the palette before.
        /// </summary>
        /// <param name="combo"></param>
        /// <returns></returns>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool AddCombination(MaterialCombination combo)
        {
            if (!ToPaletteID.ContainsKey(combo))
            {
                FromPaletteID[this.Count] = combo;
                ToPaletteID[combo] = this.Count;
                return true;
            }

            return false;
        }

        public List<MaterialCombination> ToCombinationList() => FromPaletteID.Values.ToList();

        /// <summary>
        /// Produces the full list of combinations that are valid for the given options. 
        /// Useful for initializing color palettes or color mappers.
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public List<MaterialCombination> ToValidCombinationList(Options opts)
        {
            var list = this.FromPaletteID.Values
            .Where(mc => !mc.Bottom.IsAir && !mc.Top.IsAir)
            .Where(mc => mc.Bottom.CanBeUsedAsBottomLayer)
            .Where(mc => opts.IsMultiLayerRequired ? mc.IsMultiLayer : true)
            .Where(mc => mc.Bottom.IsEnabledF(opts) && mc.Top.IsEnabledF(opts))
            .Where(mc => opts.IsMultiLayer ? true : !mc.IsMultiLayer)
            .ToList();

            return list;
        }

        private static Lazy<MaterialPalette> Instance = new Lazy<MaterialPalette>(() =>
        {
            var rt = ResxHelper.LoadJson<MaterialPalette>(Resources.Data.materialPalette);
            rt.PrimePalette();
            return rt;
        });

        public static MaterialPalette FromResx() => Instance.Value;
        public Dictionary<int, string> ToResxDictionary() {
            var output = Instance.Value.FromPaletteID.ToDictionary(
                keySelector: e => e.Key,
                elementSelector: e =>
                {
                    var value = e.Value;
                    if (value == null) return null;
                    if (value.IsMultiLayer) return ($"{value.Bottom.PixelStackerID},{value.Top.PixelStackerID}");
                    else return $"{value.Bottom.PixelStackerID}";
                });
            return output;
        }

        public void PrimePalette()
        {
            foreach (var kvp in ToPaletteID)
            {
                _ = kvp.Key.Top.TopImage;
                _ = kvp.Key.Bottom.TopImage;
                _ = kvp.Key.Top.SideImage;
                _ = kvp.Key.Bottom.SideImage;
                _ = kvp.Key.SideImage;
                _ = kvp.Key.TopImage;
            }
        }

        internal static MaterialPalette FromDictionary(Dictionary<int, MaterialCombination> dic)
        {
            return new MaterialPalette()
            {
                FromPaletteID = dic,
                ToPaletteID = dic.ToDictionary(keySelector => keySelector.Value, val => val.Key)
            };
        }
    }
}
