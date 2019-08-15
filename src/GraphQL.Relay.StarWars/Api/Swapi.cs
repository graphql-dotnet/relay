using GraphQL.Relay.StarWars.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GraphQL.Relay.StarWars.Api
{
  public class Swapi
  {
    private readonly HttpClient _client;

    private const string _apiBase = "http://swapi.co/api";
    private ResponseCache _cache = new ResponseCache();


    private string GetResource<T>() where T : Entity
    {
      return typeof(T).Name.ToLower();
    }

    public Swapi(ResponseCache cache)
    {
      _cache = cache;
      _client = new HttpClient();
      _client.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue("application/json")
      );
    }

    public Task<T> Fetch<T>(Uri url) where T : ISwapiResponse
    {
      return _cache.GetOrAdd(url, async u =>
      {
        var result = await _client.GetAsync(u);
        return DeserializeObject<T>(
            await result.Content.ReadAsStringAsync()
        );
      }).ContinueWith(
          t => (T)t.Result,
          TaskContinuationOptions.OnlyOnRanToCompletion |
          TaskContinuationOptions.ExecuteSynchronously
      );
    }

    public async Task<IEnumerable<T>> FetchMany<T>(IEnumerable<Uri> urls)
        where T : Entity
    {
      var entities = await Task.WhenAll(urls.Select(Fetch<T>));
      return entities.AsEnumerable();
    }

    public async Task<T> GetEntity<T>(string id) where T : Entity
    {
      var name = GetResource<T>();
      var entity = await GetEntity<T>(new Uri($"{_apiBase}/{name}/{id}"));

      return entity;
    }

    public Task<T> GetEntity<T>(Uri url) where T : Entity =>
        Fetch<T>(url);


    public Task<IEnumerable<T>> GetMany<T>(IEnumerable<Uri> urls) where T : Entity =>
        FetchMany<T>(urls);


    private bool DoneFetching(int count, ConnectionArguments args)
    {
      if (args.After != null || args.Before != null || args.Last != null || args.First == null)
        return false;
      return count >= args.First.Value;
    }
    public async Task<List<T>> GetConnection<T>(ConnectionArguments args)
        where T : Entity
    {
      var nextUrl = new Uri($"{_apiBase}/{typeof(T).Name.ToLower()}");
      var entities = new List<T>();
      var canStopEarly =
          args.After != null ||
          args.Before != null ||
          args.Last != null ||
          args.First == null;

      EntityList<T> page;
      while (nextUrl != null && !DoneFetching(entities.Count, args))
      {
        page = await Fetch<EntityList<T>>(nextUrl);
        entities.AddRange(page.Results);
        nextUrl = page.Next;
      }

      return entities;
    }

    private T DeserializeObject<T>(string payload)
    {
      return JsonConvert.DeserializeObject<T>(
          payload,
          new JsonSerializerSettings
          {
            ContractResolver = new DefaultContractResolver
            {
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