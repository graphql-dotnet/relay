using System.Text;
using GraphQL.Relay.Todo.Schema;

namespace GraphQL.Relay.Todo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQL(b => b
                .AddApolloTracing(true)
                .AddSchema<TodoSchema>()
                .AddAutoClrMappings()
                .AddSystemTextJson()
                .AddErrorInfoProvider(options => options.ExposeExceptionStackTrace = true));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var writer = new SchemaWriter(new TodoSchema());

            // TODO: Why is it all necessary?
            string schema = writer.GenerateAsync().GetAwaiter().GetResult();
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
                .UseGraphQL()
                .UseGraphQLGraphiQL();
        }
    }
}
