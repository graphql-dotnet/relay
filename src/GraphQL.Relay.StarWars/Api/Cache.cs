
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Relay.StarWars.Api;

namespace GraphQL.Relay.StarWars.Api
{
  public class ResponseCache : ConcurrentDictionary<Uri, Task<ISwapiResponse>>
  {

    // public Task<TSwapi> Add<TSwapi>(Uri key, Task<TSwapi> response)
    //   where TSwapi : ISwapiResponse
    // {
    //   this.GetOrAdd
    // }
    // public TSwapi GetEntity<TSwapi>(Uri key) where TSwapi : ISwapiResponse {
    //   return (TSwapi)this[key];
    // }
  }
}