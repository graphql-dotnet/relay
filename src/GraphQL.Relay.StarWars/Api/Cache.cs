using System.Collections.Concurrent;

namespace GraphQL.Relay.StarWars.Api
{
    public class ResponseCache : ConcurrentDictionary<Uri, Task<ISwapiResponse>>
    {
    }
}
