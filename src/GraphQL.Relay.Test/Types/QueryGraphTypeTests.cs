namespace GraphQL.Relay.Test.Types
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using GraphQL.Relay.Types;
    using GraphQL.Types;
    using Xunit;
    using Shouldly;
    using GraphQL.SystemTextJson;

    public class SimpleData
    {
        public string Id { get; set; }

        public static IEnumerable<SimpleData> GetData() => new List<SimpleData> {
            new SimpleData { Id = "1" },
            new SimpleData { Id = "Banana" },
            new SimpleData { Id = "71dfed33-67a8-4546-bb62-e1989bb99652" },
            new SimpleData { Id = "c17b1a3c-de5c-44a7-b368-f99bd320f508" }
        };
    }

    public class SimpleNodeGraphType : NodeGraphType<SimpleData>
    {
        public SimpleNodeGraphType() : base()
        {
            Name = "SimpleNode";

            Id(x => x.Id);
        }

        public override SimpleData GetById(string id, IResolveFieldContext<object> context) => SimpleData
            .GetData()
            .FirstOrDefault(x => x.Id.Equals(id));

    }

    public class SimpleSchema : Schema
    {
        public SimpleSchema() => (Query) = (new QueryGraphType());
    }

    public class QueryGraphTypeTests
    {
        public Schema Schema { get; }

        public DocumentExecuter Executer { get; }

        public DocumentWriter DocumentWriter { get; }

        public QueryGraphTypeTests() =>
            (Schema, Executer, DocumentWriter) = (new SimpleSchema(), new DocumentExecuter(), new DocumentWriter());

        /// <summary>
        /// Tests the implementation of the "node" query
        /// Tests the arguments and naming
        /// </summary>
        [Fact]
        public void Node_ShouldImplementNodeQuery()
        {
            var type = new QueryGraphType();

            // should have node query
            type.HasField("node").ShouldBeTrue();
            // query has one argument
            type.GetField("node").Arguments.Count.ShouldBe(1);
            // query has argument 
            type.GetField("node").Arguments[0].Name.ShouldBe("id");
            type.GetField("node").Arguments[0].Type.ShouldBe(typeof(NonNullGraphType<IdGraphType>));
        }

        /// <summary>
        /// Tests the default name
        /// </summary>
        [Fact]
        public void Name_ShouldSetDefaultName()
        {
            var type = new QueryGraphType();

            type.Name.ShouldNotBeEmpty();
            type.Name.ShouldBe("Query");
        }

        /// <summary>
        /// Tests the "node" query
        /// Should return a node by it's global ID
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData(
            @"{ node(id: ""U2ltcGxlTm9kZTox"") { id } }",
            "{\"data\":{\"node\":{\"id\":\"U2ltcGxlTm9kZTox\"}}}"
        )]
        [InlineData(
            @"{ node(id: ""U2ltcGxlTm9kZTpCYW5hbmE="") { id } }",
            "{\"data\":{\"node\":{\"id\":\"U2ltcGxlTm9kZTpCYW5hbmE=\"}}}"
        )]
        [InlineData(
            @"{ node(id: ""U2ltcGxlTm9kZTo3MWRmZWQzMy02N2E4LTQ1NDYtYmI2Mi1lMTk4OWJiOTk2NTI="") { id } }",
            "{\"data\":{\"node\":{\"id\":\"U2ltcGxlTm9kZTo3MWRmZWQzMy02N2E4LTQ1NDYtYmI2Mi1lMTk4OWJiOTk2NTI=\"}}}"
        )]
        [InlineData(
            @"{ node(id: ""U2ltcGxlTm9kZTpjMTdiMWEzYy1kZTVjLTQ0YTctYjM2OC1mOTliZDMyMGY1MDg="") { id } }",
            "{\"data\":{\"node\":{\"id\":\"U2ltcGxlTm9kZTpjMTdiMWEzYy1kZTVjLTQ0YTctYjM2OC1mOTliZDMyMGY1MDg=\"}}}"
        )]
        public async Task NodeQuery_ShouldReturnNodeForId(string query, string expected)
        {
            var result = await Executer.ExecuteAsync(options =>
            {
                options.Schema = Schema;
                options.Query = query;
            });

            var writtenResult = await DocumentWriter.WriteToStringAsync(result);
            writtenResult.ShouldBe(expected);
        }
    }
}