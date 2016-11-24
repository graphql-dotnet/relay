using GraphQL.Language.AST;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Relay.Types
{
    public class MutationInputGraphType : InputObjectGraphType
    {
        public MutationInputGraphType()
        {
            Field<StringGraphType>("clientMutationId");
        }
    }
}
