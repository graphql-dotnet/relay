namespace GraphQL.Relay.Todo.Schema
{
    public class TodoSchema : GraphQL.Types.Schema
    {
        public TodoSchema()
        {
            Query = new TodoQuery();
            Mutation = new TodoMutation();
        }
    }
}
