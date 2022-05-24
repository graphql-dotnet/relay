using GraphQL.MicrosoftDI;
using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.StarWars.Types;
using GraphQL.Relay.Types;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types.Relay;

namespace GraphQL.Relay.StarWars
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
            services
                .AddTransient<NodeInterface>()
                .AddTransient<PageInfoType>()
                .AddTransient<Swapi>()
                .AddSingleton<ResponseCache>();

            services.AddGraphQL(b => b
                .AddMetrics(true)
                .AddHttpMiddleware<StarWarsSchema>()
                .AddSchema<StarWarsSchema>()
                .AddSystemTextJson()
                .AddErrorInfoProvider(options => options.ExposeExceptionStackTrace = true)
                .AddGraphTypes(typeof(StarWarsSchema).Assembly));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQL<StarWarsSchema>();
            app.UseGraphQLGraphiQL();
        }
    }
}
