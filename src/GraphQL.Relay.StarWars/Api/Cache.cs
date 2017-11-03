
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Relay.StarWars.Api;

namespace GraphQL.Relay.StarWars.Api
{
  public class ResponseCache : ConcurrentDictionary<Uri, Task<ISwapiResponse>>
  {
  }
}
