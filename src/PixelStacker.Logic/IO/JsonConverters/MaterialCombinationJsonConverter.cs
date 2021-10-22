using Newtonsoft.Json;
using PixelStacker.Logic.Model;
using System;

namespace PixelStacker.Logic.IO.JsonConverters
{
    public class MaterialCombinationJsonConverter : JsonConverter<MaterialCombination>
    {
        public override MaterialCombination ReadJson(JsonReader reader, Type objectType, MaterialCombination existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String) return null;
            string val = reader.Value as string;
            string[] ids = val?.Split(',');

            if (ids.Length == 1) return new MaterialCombination(ids[0]);
            else if (ids.Length == 2) return new MaterialCombination(ids[0], ids[1]);
            else return null;
        }

        public override void WriteJson(JsonWriter writer, MaterialCombination value, JsonSerializer serializer)
        {
            if (value == null) writer.WriteNull();
            if (value.IsMultiLayer) writer.WriteValue($"{value.Bottom.PixelStackerID},{value.Top.PixelStackerID}");
            else writer.WriteValue($"{value.Bottom.PixelStackerID}");
        }
    }
}
