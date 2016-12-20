using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GraphQL.Relay.Utilities
{
    public class InputConverter : JsonConverter
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Dictionary<string, object>)
                .GetTypeInfo()
                .IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException($"{GetType().Name} should only be used while deserializing.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var value = serializer.Deserialize(reader);
            var dict = GetValue(value) as Dictionary<string, object>;

            return new Inputs(dict ?? new Dictionary<string, object>());
        }

        // copied from: https://github.com/graphql-dotnet/graphql-dotnet/blob/1f486e52f5891d9e1202d3b05b360644ab189ed1/src/GraphQL/StringExtensions.cs#L80
        // https://github.com/graphql-dotnet/graphql-dotnet/blob/master/LICENSE.md
        private static object GetValue(object value)
        {
            var objectValue = value as JObject;
            if (objectValue != null)
            {
                var output = new Dictionary<string, object>();
                foreach (var kvp in objectValue)
                {
                    output.Add(kvp.Key, GetValue(kvp.Value));
                }
                return output;
            }

            var propertyValue = value as JProperty;
            if (propertyValue != null)
            {
                return new Dictionary<string, object>
                {
                    { propertyValue.Name, GetValue(propertyValue.Value) }
                };
            }

            var arrayValue = value as JArray;
            if (arrayValue != null)
            {
                return arrayValue.Children().Aggregate(new List<object>(), (list, token) =>
                {
                    list.Add(GetValue(token));
                    return list;
                });
            }

            var rawValue = value as JValue;
            if (rawValue != null)
            {
                var val = rawValue.Value;
                if (val is long)
                {
                    long l = (long)val;
                    if (l >= int.MinValue && l <= int.MaxValue)
                    {
                        return (int)l;
                    }
                }
                return val;
            }

            return value;
        }
    }
}
