using System.Threading.Tasks;
using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;
using GraphQL.Types;

namespace GraphQL.Relay.StarWars.Types
{
  public class PeopleGraphType : NodeGraphType<People, Task<People>>
  {
    private readonly Swapi _api;
    public PeopleGraphType(Swapi api)
    {
        _api = api;

        Name = "People";

        Id(p => p.Id);
        Field(p => p.Name);
        Field(p => p.Height);
        Field(p => p.Mass);
        Field(p => p.HairColor);
        Field(p => p.SkinColor);
        Field(p => p.EyeColor);
        Field(p => p.BirthYear);
        Field(p => p.Gender);
        Field(p => p.Homeworld);
    }

    public override Task<People> GetById(string id) =>
        _api.Get<People>(id);

  }
}