using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Relay.Http;
using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.StarWars.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Scrutor;
using Microsoft.Extensions.Hosting;
using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Types.Relay;
using GraphQL.Relay.Types;
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
                .AddTransient(typeof(ConnectionType<>))
                .AddTransient(typeof(EdgeType<>))
                .AddTransient<NodeInterface>()
                .AddTransient<PageInfoType>()
                .AddSingleton<StarWarsSchema>();

            services
                .AddGraphQL(options =>
                {
                    options.EnableMetrics = false;
                })
                .AddSystemTextJson()
                .AddRelayGraphTypes()
                .AddGraphTypes(typeof(StarWarsSchema));

            services.AddTransient<Swapi>();
            services.AddSingleton<ResponseCache>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQL<StarWarsSchema>();
        }
    }
}
