using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Http;

namespace GraphQL.Relay.Http
{
    public class RelayResponse
    {
        public IDocumentWriter Writer { get; set; }
        public bool IsBatched { get; set; }
        public HttpRequestMessage Request { get; set; }
        public IEnumerable<ExecutionResult> Results { get; set; }

        public IEnumerable<ExecutionError> Errors => Results?.SelectMany(r => r.Errors);


        public HttpResponseMessage Write()
        {
            string content;
            var statusCode = HttpStatusCode.OK;

            if (Results.Any(r => r.Errors?.Count > 0))
                statusCode = HttpStatusCode.BadRequest;

            try
            {
                content = Writer.Write(IsBatched ? (object)Results : Results.FirstOrDefault());
            }
            catch (Exception err)
            {
                statusCode = HttpStatusCode.InternalServerError;
                content = $"{{ \"errors\": [\"{err.Message}\"] }}";
            }

            return new HttpResponseMessage
            {
                StatusCode = statusCode,
                RequestMessage = Request,
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
        }
    }
}
