using System.Collections.Generic;
using System.Linq;
using GraphQL.Http;

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

    public string Write()
    {
      return Writer.Write(IsBatched ?
          (object)Results :
          Results.FirstOrDefault()
      );
    }
  }
}
