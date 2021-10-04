using Newtonsoft.Json;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;

namespace PixelStacker.IO.JsonConverters
{
    public class MaterialPaletteJsonConverter : JsonConverter<MaterialPalette>
    {
        public override MaterialPalette ReadJson(JsonReader reader, Type objectType, MaterialPalette existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var dic = serializer.Deserialize<Dictionary<int, MaterialCombination>>(reader);
            return MaterialPalette.FromDictionary(dic);
        }

        public override void WriteJson(JsonWriter writer, MaterialPalette value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.FromPaletteID);
        }
    }
}
