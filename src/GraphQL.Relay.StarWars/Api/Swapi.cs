using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GraphQL.Relay.StarWars.Api
{
    public class Swapi
    {
        private readonly HttpClient _client;

        private string _apiBase = "http://swapi.co/api";
        private ResponseCache _cache = new ResponseCache();

        public Swapi(ResponseCache cache)
        {
            _cache = cache;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        private async Task<T> Get<T>(Uri url) where T : Entity
        {
            if (_cache.ContainsKey(url)) return _cache.GetEntity<T>(url);
            var result = await _client.GetAsync(url);
            var entity = DeserializeObject<T>(
                await result.Content.ReadAsStringAsync()
            );
            _cache[url] = entity;
            return entity;
        }

        public async Task<T> Get<T>(string id) where T : Entity
        {
            var name = typeof(T).Name.ToLower();
            var entity = await Get<T>(new Uri($"{_apiBase}/{name}/{id}"));

            return entity;
        }

        // public async Task<T[]> GetMany<T>(IEnumerable<string> ids)
        //     where T : Entity
        // {
        //     return await Task.WhenAll(
        //         ids.Select(id => Get<T>(id))
        //     );
        // }

        public async Task<T[]> GetMany<T>(IEnumerable<Uri> urls)
            where T : Entity
        {
            var entities = await Task.WhenAll(urls.Select(Get<T>));
            return entities;
        }

        public async Task<EntityList<T>> GetAll<T>()
            where T : Entity
        {
            var name = typeof(T).Name.ToLower();

            var result = await _client.GetAsync($"{_apiBase}/{name}");
            var entity = DeserializeObject<EntityList<T>>(
                await result.Content.ReadAsStringAsync()
            );

            return entity;
        }

        private T DeserializeObject<T>(string payload) {
            return JsonConvert.DeserializeObject<T>(
                payload,
                new JsonSerializerSettings {
                    ContractResolver = new DefaultContractResolver {
                         NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    Converters = new List<JsonConverter> {
                         new NumberConverter()
                    }
                }
            );
        }
    }

    public class NumberConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(int))
                return int.Parse(reader.Value.ToString());

            if (objectType == typeof(double))
                return double.Parse(reader.Value.ToString());

            return reader.Value;
        }

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(int) || objectType == typeof(double);

    }
}