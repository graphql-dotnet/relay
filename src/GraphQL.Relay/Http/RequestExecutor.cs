using System;
using System.Collections.Generic;
using System.IO;
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
            Stream body,
            string contentType,
            Action<GraphQL.ExecutionOptions, IEnumerable<HttpFile>> configure
        ) {
            var queries = await Deserializer.Deserialize(body, contentType);

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
                Results = results
            };
        }

        public async Task<RelayResponse> ExecuteAsync(
            HttpRequestMessage request,
            Action<GraphQL.ExecutionOptions, IEnumerable<HttpFile>> configure
        ) {
            return await ExecuteAsync(
                await request.Content.ReadAsStreamAsync(),
                request.Content.Headers.ContentType.MediaType,
                configure
            );
        }
    }
}
