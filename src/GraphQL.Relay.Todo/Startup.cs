<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Conversion;
=======
using GraphQL.Relay.Todo.Schema;
using GraphQL.Server;
>>>>>>> Stashed changes
using GraphQL.SystemTextJson;
using GraphQL.Relay.Todo.Schema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
<<<<<<< Updated upstream
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using GraphQL.Types;
using GraphQL.Server;
=======
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Text;
using System.Threading.Tasks;
>>>>>>> Stashed changes

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
            services.AddCors(opt => opt.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

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
            app.UseCors("AllowAll");

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
