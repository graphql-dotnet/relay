using GraphQL.Relay.Types;
using Shouldly;
using Xunit;

namespace GraphQL.Relay.Tests.Types
{
    public class NodeTypeTests
    {
        public class Droid
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public abstract class DroidType<TOut> : NodeGraphType<Droid, TOut>
        {
            public DroidType()
            {
                Name = "Droid";
                Id(d => d.Id);
            }
        }

        public class DroidType : DroidType<Droid>
        {
            public override Droid GetById(IResolveFieldContext<object> context, string id)
            {
                return new Droid { Id = id, Name = "text" };
            }
        }

        public class DroidTypeAsync : DroidType<Task<Droid>>
        {
            public override async Task<Droid> GetById(IResolveFieldContext<object> context, string id)
            {
                await Task.Delay(0);
                return new Droid { Id = id, Name = "text" };
            }
        }

        [Fact]
        public void it_should_handle_id_conflict()
        {
            var type = new DroidType();

            type.Fields.Count().ShouldBe(2);
            type.HasField("id").ShouldBeTrue();
            type.HasField("droidId").ShouldBeTrue();
        }

        [Fact]
        public async void it_should_allow_async()
        {
            var type = new DroidTypeAsync();

            var droid = await type.GetById(null, "3");
            droid.Id.ShouldBe("3");
        }
    }
}
