using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace GraphQL.Relay.StarWars.Api
{
    public class ResponseCache : ConcurrentDictionary<Uri, Task<ISwapiResponse>>
    {
    }
}
