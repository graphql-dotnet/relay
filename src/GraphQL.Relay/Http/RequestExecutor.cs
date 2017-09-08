using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQL.Http;

namespace GraphQL.Relay.Http
{
    public class RequestExecutor
    {
        private readonly IDocumentExecuter _executer = new DocumentExecuter();
        private readonly IDocumentWriter _writer = new DocumentWriter();

        public RequestExecutor()
        {    
        }

        public RequestExecutor(IDocumentExecuter executer, IDocumentWriter writer)
        {
            _executer = executer;
            _writer = writer;
        }

        public async Task<RelayResponse> ExecuteAsync(
            HttpRequestMessage request, 
            Action<GraphQL.ExecutionOptions, IEnumerable<HttpFile>> configure
        ) {
            var queries = await Deserializer.Deserialize(request.Content);

            var results = await Task.WhenAll(
                queries.Select(q => _executer.ExecuteAsync(options =>
                {
                    options.Query = q.Query;
                    options.OperationName = q.OperationName;
                    options.Inputs = q.Variables;
   
                    configure(options, queries.Files);
                }))
            );

            return new RelayResponse
            {
                Writer = _writer,
                IsBatched = queries.IsBatched,
                Request = request,
                Results = results
            };
        }
    }
}
