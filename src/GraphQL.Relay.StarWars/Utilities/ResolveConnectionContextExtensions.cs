using GraphQL.Builders;

namespace GraphQL.Relay.StarWars.Utilities
{
    public class ConnectionArguments {
        public int? First { get; set; }
        public string After { get; set; }
        public int? Last { get; set; }
        public string Before { get; set; }
    }
    public static class ResolveConnectionContextExtensions
    {
        public static ConnectionArguments GetConnectionArguments<T>(this ResolveConnectionContext<T> ctx) {
            return new ConnectionArguments {
                First = ctx.First,
                After = ctx.After,
                Last = ctx.Last,
                Before = ctx.Before,
            };
        }
    }
}