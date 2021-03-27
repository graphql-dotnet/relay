using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Conversion;
using GraphQL.SystemTextJson;
using GraphQL.Relay.Http;
using GraphQL.Relay.Todo.Schema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace GraphQL.Relay.Todo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<IDocumentWriter, DocumentWriter>();
            services.AddScoped<RequestExecutor>();
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var writer = new SchemaWriter(new TodoSchema());

            string schema = await writer.Generate();
            using (FileStream fs = File.Create(Path.Combine(env.WebRootPath, "schema.json")))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(schema);
                fs.Write(info, 0, info.Length);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseStaticFiles()
                .UseDefaultFiles()
                .UseRouter(
                    new RouteBuilder(app)
                        .MapPost("graphql", async context =>
                        {
                            var executor = context.RequestServices.GetService<RequestExecutor>();
                            try
                            {
                                var resp = await executor.ExecuteAsync(
                                    context.Request.Body,
                                    context.Request.ContentType,
                                    (_, files) =>
                                    {
                                        _.Schema = new TodoSchema();
                                        //_.FieldNameConverter = new CamelCaseFieldNameConverter();
                                    }
                                );

                                await context.Response.WriteAsync(await resp.Write());

                            }
                            catch (Exception err)
                            {
                                throw err;
                            }
                        })
                        .Build()
                );
        }
    }
}
