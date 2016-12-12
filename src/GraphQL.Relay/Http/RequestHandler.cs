using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Http;
using GraphQL;
using GraphQL.Types;
using System.Net.Http;
using System.Net;

namespace GraphQL.Relay.Http
{
    public class ExecutionOptions
    {
        public ISchema Schema;
        public object RootContext;
        public object UserContext;
    }

    public class RelayRootContext
    {
        public IEnumerable<HttpFile> Files { get; set; }
    }

    public class RequestHandler
    {
        private readonly IDocumentWriter writer = new DocumentWriter();
        private readonly IDocumentExecuter executor = new DocumentExecuter();

        Func<RelayRequest, ExecutionOptions> executionOptions;

        public RequestHandler(Func<RelayRequest, ExecutionOptions> getOptions)
        {
            executionOptions = getOptions;
        }

        public RequestHandler(ISchema schema, object rootObject = null, object userContext = null)
        {
            executionOptions = req => new ExecutionOptions
            {
                Schema = schema,
                UserContext = userContext,
                RootContext = rootObject ?? new RelayRootContext { Files = req.Files },
            };  
        }

        public async Task<HttpResponseMessage> Handle(HttpRequestMessage request)
        {
            string content = "";
            HttpStatusCode statusCode = HttpStatusCode.OK;
            RelayRequest queries = await Deserializer.Deserialize(request.Content);

            var options = executionOptions(queries);

            if (options.RootContext != null && options.RootContext is RelayRootContext)
            {
                RelayRootContext ctx = options.RootContext as RelayRootContext;
                if (ctx.Files == null)
                    ctx.Files = queries.Files;
            }

            var result = await Task.WhenAll(
                queries.Select(q => executor.ExecuteAsync(
                    options.Schema,
                    options.RootContext,
                    query: q.Query,
                    inputs: q.Variables,
                    userContext: options.UserContext,
                    operationName: q.OperationName
                ))
            );

            if (Enumerable.Any(result, r => r.Errors?.Count > 0))
                statusCode = HttpStatusCode.BadRequest;

            try {
                content = writer.Write(queries.IsBatched
                    ? (object)result
                    : result[0]
                );
            }
            catch (Exception err)
            {
                statusCode = HttpStatusCode.InternalServerError;
                content = $"{{ \"errors\": [\"{err.Message}\"] }}";
            }

            return new HttpResponseMessage
            {
                StatusCode = statusCode,
                RequestMessage = request,
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
        }   
    }
}
