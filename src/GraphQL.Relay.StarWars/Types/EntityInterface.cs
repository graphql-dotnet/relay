using GraphQL.Types;

namespace GraphQL.Relay.StarWars.Types
{
    public class EntityInterface : InterfaceGraphType
    {
        public EntityInterface()
        {
            Field<IdGraphType>("id");
            Field<DateGraphType>("created");
            Field<DateGraphType>("edited");
            Field<StringGraphType>("url");
        }
    }
}
