using System.IO;
using System.Text;
using GraphQL.Relay.Todo.Schema;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace GraphQL.Relay.Todo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<IDocumentWriter, DocumentWriter>();
            services.AddGraphQL()
                .AddSystemTextJson();
            services.AddSingleton<TodoSchema>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app
               .UseStaticFiles()
               .UseDefaultFiles()
               .UseGraphQL<TodoSchema>()
               .UseGraphQLGraphiQL();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
