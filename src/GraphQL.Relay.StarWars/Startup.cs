using GraphQL.Http;
using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.StarWars.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddMvc();

            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<IDocumentWriter, DocumentWriter>();
            services.AddScoped<Relay.Http.RequestExecutor>();

            services.AddScoped<Swapi>();
            services.AddSingleton<ResponseCache>();

            services.AddScoped<StarWarsQuery>();
            services.AddScoped<FilmGraphType>();
            services.AddScoped<PeopleGraphType>();
            services.AddScoped<PlanetGraphType>();
            services.AddScoped<SpeciesGraphType>();
            services.AddScoped<StarshipGraphType>();
            services.AddScoped<VehicleGraphType>();

            services.AddScoped(provider => new StarWarsSchema(provider));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc().UseStaticFiles();
        }
    }
}
