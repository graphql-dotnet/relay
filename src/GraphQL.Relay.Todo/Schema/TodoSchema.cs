<<<<<<< Updated upstream:src/GraphQL.Relay.Todo/Schema/Schema.cs
using System;
using GraphQL.Types;
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
>>>>>>> Stashed changes:src/GraphQL.Relay.Todo/Schema/TodoSchema.cs

namespace GraphQL.Relay.Todo.Schema
{
    public class TodoSchema: GraphQL.Types.Schema
    {
        public TodoSchema()
        {
            Query = new TodoQuery();
            Mutation = new TodoMutation();
        }
    }
}
