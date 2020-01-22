using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Full.Pirate.Library.DbContexts;
using Full.Pirate.Library.Services;
using Full.Pirate.Library.Services.Sorting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

namespace Full.Pirate.Library
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IActionResult ValidationErrorData(ActionContext context)
        {
           var problemDetails = new ValidationProblemDetails(context.ModelState)
           {
               Status = StatusCodes.Status422UnprocessableEntity,
               Instance = context.HttpContext.Request.Path,
               Type = "https://piratelibrary.com/modelvalidationproblem",
               Title = "One or more validation errors occurred.",
               Detail = "Shit, fix it."

           };
            problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
            return new UnprocessableEntityObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true; //only returns data in content type requested (like xml) or else error
             })
            .ConfigureApiBehaviorOptions(setupAction =>
            {
                setupAction.InvalidModelStateResponseFactory = 
                context => ValidationErrorData(context);
            })
            .AddNewtonsoftJson(setupAction => //use camel case in patch document
            {
                setupAction.SerializerSettings.ContractResolver =
                   new CamelCasePropertyNamesContractResolver();
            }).AddXmlDataContractSerializerFormatters() //can return data as xml
            ;
            services.AddTransient<IPropertyMappingService, PropertyMappingService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IRepositoryService, RepositoryService>();
           
            services.AddDbContext<PirateLibraryContext>(options =>
            {
                var dbConnSection = Configuration.GetSection("ConnectionStrings");
                var dbConnString = dbConnSection.GetValue<string>("DbConnection");
                options.UseSqlServer(dbConnString, options =>
                {
                    options.CommandTimeout(25);
                });
             });
    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(configure=> {
                    configure.Run(async handler =>
                    {
                        handler.Response.StatusCode = 500;
                        await handler.Response.WriteAsync("AN error occured. Try again later");
                    });
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
