using Newtonsoft.Json;
using System;
using System.Drawing;
using PixelStacker.Extensions;
using System.IO;
using System.Drawing.Imaging;
using SkiaSharp;

namespace PixelStacker.Logic.IO.JsonConverters
{
    public class SKColorJsonTypeConverter : JsonConverter<SKColor>
    {
        public override SKColor ReadJson(JsonReader reader, Type objectType, SKColor existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String) return default;
            string b64 = reader.Value as string;
            if (string.IsNullOrEmpty(b64)) return default;
            
            if (SKColor.TryParse(b64, out SKColor color)){
                return color;
            }

            return new SKColor(0, 0, 0, 255);
        }

        public override void WriteJson(JsonWriter writer, SKColor value, JsonSerializer serializer)
        {
            var bm = value.ToString();
            writer.WriteValue(bm);
        }
    }
}
