using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (IsBatched)
                return WriteToStringAsync(Writer, Results.ToArray()).GetAwaiter().GetResult();
            else
                return Writer.WriteToStringAsync(Results.FirstOrDefault()).GetAwaiter().GetResult();
        }

        private static readonly Encoding Utf8Encoding = new UTF8Encoding(false);

        //TODO: remove later
        private static async Task<string> WriteToStringAsync(
            IDocumentWriter writer,
            ExecutionResult[] value)
        {
            using (var stream = new MemoryStream())
            {
                await writer.WriteAsync(stream, value);
                stream.Position = 0;
                using (var reader = new StreamReader(stream, Utf8Encoding))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
