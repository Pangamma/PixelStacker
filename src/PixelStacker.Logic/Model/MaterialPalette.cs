using Newtonsoft.Json;
using PixelStacker.IO.JsonConverters;
using PixelStacker.Resources;
using PixelStacker.Resources.Localization;
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
        
        public static MaterialPalette FromResx()
        {
            var rt = ResxHelper.LoadJson<MaterialPalette>(DataResources.materialPalette);
            return rt;
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
