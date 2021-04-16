using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Relay.Types;
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
            public override Droid GetById(string id, IResolveFieldContext<object> context)
            {
                return new Droid { Id = id, Name = "text" };
            }
        }

        public class DroidTypeAsync : DroidType<Task<Droid>>
        {
            public override async Task<Droid> GetById(string id, IResolveFieldContext<object> context)
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

            var droid = await type.GetById("3", null);
            droid.Id.ShouldBe("3");
        }
    }
}
