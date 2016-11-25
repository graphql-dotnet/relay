using GraphQL;
using GraphQL.Relay.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Relay.Http
{
    public class HttpFile
    {
        public string ContentDisposition { get; set; }
        public string ContentType { get; set; }
        public Stream Data { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
    }

    public class RelayRequest : List<RelayQuery>
    {
        public bool IsBatched { get; private set; } = false;
    
        public IEnumerable<HttpFile> Files { get; set; }

        public RelayRequest() {}

        public RelayRequest(bool isBatched) : base()
        {
            IsBatched = isBatched;
        }

        public RelayRequest(IEnumerable<RelayQuery> list, bool isBatched) : base(list)
        {
            IsBatched = isBatched;
        }
    }

    public class RelayQuery
    {
        public string OperationName { get; set; }
        public string Query { get; set; }

        [JsonConverter(typeof(InputConverter))]
        public Inputs Variables { get; set; }
    }
}
