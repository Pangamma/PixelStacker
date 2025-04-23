using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace PixelStacker.CodeGenerator.BlockRipper
{
    public class SingleOrArray<T>
    {
        private List<T> _values;

        public SingleOrArray(T value)
        {
            _values = new List<T> { value };
        }

        public SingleOrArray(IEnumerable<T> values)
        {
            _values = new List<T>(values);
        }

        public IEnumerable<T> Values => _values;

        public static implicit operator SingleOrArray<T>(T value) => new SingleOrArray<T>(value);
        public static implicit operator SingleOrArray<T>(T[] values) => new SingleOrArray<T>(values);
        public static implicit operator SingleOrArray<T>(List<T> values) => new SingleOrArray<T>(values);
    }

    public class SingleOrArrayConverter<T> : JsonConverter<SingleOrArray<T>>
    {
        public override SingleOrArray<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Check if the JSON token is an array or a single value
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var list = JsonSerializer.Deserialize<List<T>>(ref reader, options);
                return new SingleOrArray<T>(list);
            }
            else
            {
                var singleValue = JsonSerializer.Deserialize<T>(ref reader, options);
                return new SingleOrArray<T>(singleValue);
            }
        }

        public override void Write(Utf8JsonWriter writer, SingleOrArray<T> value, JsonSerializerOptions options)
        {
            // Check if the SingleOrArray contains a single value or multiple values
            var values = value.Values;
            if (values is List<T> list && list.Count == 1)
            {
                JsonSerializer.Serialize(writer, list[0], options);
            }
            else
            {
                JsonSerializer.Serialize(writer, values, options);
            }
        }
    }


    public class SingleOrArrayMapConverter<T> : JsonConverter<Dictionary<string, SingleOrArray<T>>>
    {
        public override Dictionary<string, SingleOrArray<T>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var variants = new Dictionary<string, SingleOrArray<T>>();

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException("Expected StartObject token");

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Expected PropertyName token");

                string key = reader.GetString();
                reader.Read();

                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    var list = JsonSerializer.Deserialize<List<T>>(ref reader, options);
                    if (key == "")
                    {
                        variants["*"] = list;
                    }
                    else
                    {
                        variants[key] = list;
                    }
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var variant = JsonSerializer.Deserialize<T>(ref reader, options);
                    if (key == "")
                    {
                        variants["*"] = new List<T>() { variant };
                    }
                    else
                    {
                        variants[key] = new List<T>() { variant };
                    }
                }
                else
                {
                    throw new JsonException("Unexpected token type");
                }
            }

            return variants;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, SingleOrArray<T>> dic, JsonSerializerOptions options)
        {
            if (dic == null)
            {
                JsonSerializer.Serialize(writer, null, options);
                return;
            }

            if (dic.Count == 0)
            {
                writer.WriteStartObject();
                writer.WriteEndObject();
                return;
            }

            writer.WriteStartObject();

            foreach(var kvp in dic)
            {
                if (kvp.Key == "*")
                {
                    writer.WritePropertyName("");
                } 
                else
                {
                    writer.WritePropertyName(kvp.Key);
                }

                var values = kvp.Value.Values;
                if (values is List<T> list && list.Count == 1)
                {
                    JsonSerializer.Serialize(writer, list[0], options);
                }
                else
                {
                    JsonSerializer.Serialize(writer, values, options);
                }
            }

            writer.WriteEndObject();
        }
    }

}
