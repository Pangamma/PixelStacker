using Newtonsoft.Json;
using PixelStacker.Logic.Model;
using System;
using System.Collections.Generic;

namespace PixelStacker.Logic.IO.JsonConverters
{
    public class MaterialPaletteJsonConverter : JsonConverter<MaterialPalette>
    {
        public override MaterialPalette ReadJson(JsonReader reader, Type objectType, MaterialPalette existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var dic = serializer.Deserialize<Dictionary<int, string>>(reader);
            var convertedDictionary = new Dictionary<int, MaterialCombination>();
            foreach(var kvp in dic)
            {
                try
                {
                    string val = kvp.Value as string;
                    string[] ids = val?.Split(',');

#pragma warning disable CS0618 // Type or member is obsolete
                    MaterialCombination mc;
                    if (ids.Length == 1) mc = new MaterialCombination(ids[0]);
                    else if (ids.Length == 2) mc = new MaterialCombination(ids[0], ids[1]);
                    else mc = null;
#pragma warning restore CS0618 // Type or member is obsolete

                    if (mc != null)
                    {
                        convertedDictionary[kvp.Key] = mc;
                    } 
                    else
                    {
                        Console.Error.WriteLine($"Could not load MaterialCombination: {kvp.Value}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Could not load MaterialCombination: {kvp.Value}", ex);
                }
            }

            return MaterialPalette.FromDictionary(convertedDictionary);
        }

        public override void WriteJson(JsonWriter writer, MaterialPalette value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value?.FromPaletteID);
        }
    }
}
