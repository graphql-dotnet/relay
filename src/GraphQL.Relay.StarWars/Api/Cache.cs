
using System;
using System.Collections.Generic;
using GraphQL.Relay.StarWars.Api;

namespace GraphQL.Relay.StarWars.Api
{
  public class ResponseCache : Dictionary<Uri, Entity>
  {

    public TEntity GetEntity<TEntity>(Uri key) where TEntity : Entity {
      return (TEntity)this[key];
    }
  }
}