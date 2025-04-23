using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelStacker.CodeGenerator.BlockRipper
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Text.Json;
    using System.Text.Json.Serialization;


    public class BlockStateDefinition
    {
        [JsonConverter(typeof(SingleOrArrayMapConverter<Variant>))]
        public Dictionary<string, SingleOrArray<Variant>> Variants { get; set; }
        public List<Multipart> Multipart { get; set; }

        public List<string> GetAllPossibleModels()
        {
            List<string> output = new List<string>();
            output.AddRange(this.Variants?.Values.SelectMany(x => x.Values).Select(x => x.Model) ?? new List<string>());
            output.AddRange(this.Multipart?.SelectMany(x => x.Apply.Values).Select(x => x.Model) ?? new List<string>());
            return output.Distinct().ToList();
        }
    }

    public class Variant
    {
        private string _model;
        public string Model
        {
            get => _model;
            set
            {
                if (value != null && !value.Contains(":"))
                {
                    _model = "minecraft:" + value;
                }
                else
                {
                    _model = value;
                }
            }
        }

        public int? X { get; set; }
        public int? Y { get; set; }
        public bool? UVLock { get; set; }
        public int? Weight { get; set; }
    }

    public class Multipart
    {
        [JsonConverter(typeof(SingleOrArrayConverter<Variant>))]
        public SingleOrArray<Variant> Apply { get; set; }
        public When When { get; set; }

    }

    // Thanks Mojang. Thanks for making me have to do this.
    [JsonConverter(typeof(WhenConverter))]
    public class When : Dictionary<string, string>
    {
        public List<When> OR { get; set; }
        public List<When> AND { get; set; }
    }


    // Mojang developers are TERRIBLE at creating maintainable data schemas.
    // Thanks Chat GPT for writing this for me so I didn't have to.
    public class WhenConverter : JsonConverter<When>
    {
        public override When Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new When();

            using (var doc = JsonDocument.ParseValue(ref reader))
            {
                foreach (var property in doc.RootElement.EnumerateObject())
                {
                    switch (property.Name)
                    {
                        case "OR":
                            result.OR = JsonSerializer.Deserialize<List<When>>(property.Value.GetRawText(), options);
                            break;
                        case "AND":
                            result.AND = JsonSerializer.Deserialize<List<When>>(property.Value.GetRawText(), options);
                            break;
                        default:
                            result[property.Name] = property.Value.GetString();
                            break;
                    }
                }
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, When value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            foreach (var kvp in value)
            {
                writer.WriteString(kvp.Key, kvp.Value);
            }

            if (value.OR != null)
            {
                writer.WritePropertyName("OR");
                JsonSerializer.Serialize(writer, value.OR, options);
            }

            if (value.AND != null)
            {
                writer.WritePropertyName("AND");
                JsonSerializer.Serialize(writer, value.AND, options);
            }

            writer.WriteEndObject();
        }
    }
}
