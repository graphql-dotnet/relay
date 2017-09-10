using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Conversion;
using GraphQL.Relay.Http;
using GraphQL.Relay.StarWars.Types;
using Microsoft.AspNetCore.Mvc;

namespace GraphQL.Relay.StarWars.Controllers
{
  [Route("api/[controller]")]
  public class GraphQLController : Controller
  {
    private readonly RequestExecutor _executor;
    public StarWarsSchema Schema { get; }

    public GraphQLController(RequestExecutor executor, StarWarsSchema schema)
    {
      this.Schema = schema;
      _executor = executor;
    }

    // POST api/values
    [HttpPost]
    public async Task<IActionResult> Post()
    {
      var response = await _executor.ExecuteAsync(Request.Body, Request.ContentType, (_, files) =>
      {
        _.Schema = Schema;
        _.ExposeExceptions = true;
        _.Root = new
        {
          Files = files,
        };
        _.FieldNameConverter = new CamelCaseFieldNameConverter();
      });


      return Content(response.Write(), "application/json", Encoding.UTF8) ;
    }
  }
}
