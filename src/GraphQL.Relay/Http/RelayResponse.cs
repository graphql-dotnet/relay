using GraphQL.Http;
using System.Collections.Generic;
using System.Linq;

namespace GraphQL.Relay.Http
{
    public class RelayResponse
    {
        public IDocumentWriter Writer { get; set; }
        public bool IsBatched { get; set; }
        public IEnumerable<ExecutionResult> Results { get; set; }

        public IEnumerable<ExecutionError> Errors =>
            Results?.SelectMany(r => r.Errors) ?? new List<ExecutionError>();

        public bool HasErrors => Results.Any(r => r.Errors?.Count > 0);

        public string Write() => Writer.WriteToStringAsync(IsBatched ? (object)Results.ToArray() : Results.FirstOrDefault()).GetAwaiter().GetResult();
    }
}
