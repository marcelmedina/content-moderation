using System;
using System.IO;
using ContentModeration.Models;
using ContentModeration.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace ContentModeration
{
    public class Startup
    {
        private readonly string ApiDefinition = "Content Moderation API";
        private readonly string ApiDocXml = "ContentModeration.xml";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Settings>(options =>
            {
                Configuration.GetSection("Settings").Bind(options);
                options.OcpApimSubscriptionKey = Configuration.GetSection("Settings:Ocp-Apim-Subscription-Key").Value;
            });

            services.AddTransient<IContentModerationService, ContentModerationService>();

            services.AddMvc();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = ApiDefinition,
                    Description = "A sample created for the Auckland Azure Bootcamp",
                    Contact = new Contact
                    {
                        Name = "Marcel Medina",
                        Url = "http://sharepoint4developers.net"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, ApiDocXml);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", ApiDefinition);
            });

            app.UseMvc();
        }
    }
}
