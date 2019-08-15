using GraphQL.Conversion;
using GraphQL.Relay.Http;
using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.StarWars.Types;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Relay.StarWars.Controllers
{
  [Route("api/[controller]")]
  public class GraphQLController : Controller
  {
    private readonly RequestExecutor _executor;
    public StarWarsSchema Schema { get; }
    private readonly Swapi _api;

    public GraphQLController(RequestExecutor executor, Swapi api, StarWarsSchema schema)
    {
      Schema = schema;
      _api = api;
      _executor = executor;
    }

    // POST api/values
    [HttpPost]
    public async Task<IActionResult> Post()
    {
      var response = await _executor
        .ExecuteAsync(Request.Body, Request.ContentType, (_, files) => {
            _.Schema = Schema;
            _.ExposeExceptions = true;
            _.UserContext = new GraphQLContext(_api);

            _.Root = new
            {
              Files = files,
            };
            _.FieldNameConverter = new CamelCaseFieldNameConverter();
        });


      return Content(response.Write(), "application/json", Encoding.UTF8);
    }
  }
}
