GraphQL.Relay
===

A collection of classes, tools, and utilities for creating Relay.js compatible GraphQL servers in dotnet.

## Usage

### GraphTypes

Included are a few GraphQL types and helpers for creating Connections, Mutations, and Nodes

#### `QueryGraphType`

A top level Schema query type, which defines the required `node` root query;

```csharp
public class StarWarsQuery : QueryGraphType
{
  public StarWarsQuery() {
    Field<DroidType>(
      "hero",
      resolve: context => new Droid { DroidId = "1", Name = "R2-D2" }
    );
  }
}

public class StarWarsSchema : Schema
{
  public StarWarsSchema() {
    Query = new StarWarsQuery();
  }
}
```

#### `NodeGraphType<TSource>`

NodeTypes, are `ObjectGraphType`s that implement the [`NodeInterface`](https://facebook.github.io/relay/docs/graphql-object-identification.html#content)
and provide a `GetById` method, which allows Relay (via the `node()` Query) to refetch nodes when it needs to.

Nodes, also provide a convenient `Id()` method for defining global `id` fields, derived from the type `Name` and `Id`.
If your underlying type has a name conflict on the field `id`, the "local" Id will be prefixed with the type name,
e.g. `Droid.Id -> droidId`

```csharp
public class Droid
{
  public string DroidId { get; set; }
  public string Name { get; set; }
}

public class DroidType : NodeGraphType<Droid>
{
  public DroidType()
  {
    Name = "Droid";

    Id(d => d.DroidId); // adds an id Field as well as the `Droid` id field

    Field<StringGraphType>("name", "The name of the droid.");
  }

  public override object GetById(string droidId) {
    return StarWarsData.GetDroidByIdAsync(droidId);
  }
}
```

#### Mutations

Relay mutations specify a few constraints on top of the general GraphQL mutations. To accompodate this, there are
mutation specific GraphTypes provided.

#### `MutationGraphType`

The top level mutation type attached to the GraphQL Schema

```csharp
public class StarWarsMutation : MutationGraphType
{
  public StarWarsMutation() {

    Mutation<CreateDroidInput, CreateDroidPayload>("createDroid");
    Mutation<DeleteDroidInput, DeleteDroidPayload>("deleteDroid");
  }
}

public class StarWarsSchema : Schema
{
  public StarWarsSchema() {
    Query = new StarWarsQuery();
    Mutation = new StarWarsMutation();
  }
}
```

#### `MutationInputGraphType`

An simple base class that defines a `clientMutationId` field. functionally identical to `InputObjectGraphType` otherwise

#### `MutationPayloadGraphType<TSource>`

The output ObjectGraphType containing the mutation payload, functionally similar to an `ObjectGraphType` with the
addition of requiing a `MutateAndGetPayload()` method used to resolve the payload from the inputs.

```csharp
public class CreateDroidPayload : MutationPayloadGraphType
{
  public CreateDroidPayload()
  {
    Name = "CreateDroidPayload";

    Field(
      name: "droid",
      type: typeof(Droid));        
  }

  public override object MutateAndGetPayload(MutationInputs inputs)
  {
    string name = inputs.Get<string>("name");

    Droid newDroid = StarWarsData.CreateDroid(name)

    return new {
      Droid = newDroid
    };
  }
}
```
