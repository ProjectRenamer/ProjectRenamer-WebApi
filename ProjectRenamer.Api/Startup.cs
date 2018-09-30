using ProjectRenamer.Api.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using ProjectRenamer.Api.ConfigConstants;
using Swashbuckle.AspNetCore.Swagger;

namespace ProjectRenamer.Api
{
    public class Startup
    {
        private const string SOLUTION_NAME = "ProjectRenamer.Api";

        private const string ALLOWED_ORIGINS_SECTION_NAME = "AllowedOrigins";
        private const string SWAGGER_VERSION = "v1";


        private IConfiguration Configuration { get; }
        private IHostingEnvironment HostingEnvironment { get; }
        private IServiceCollection Services { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc(options => { options.Filters.Add<GeneralExceptionFilter>(); }).AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });
            services.AddSwaggerGen(c => { c.SwaggerDoc(SWAGGER_VERSION, new Info { Title = SOLUTION_NAME, Version = SWAGGER_VERSION }); });

            services.Configure<AllowedOriginsConstants>(Configuration.GetSection(ALLOWED_ORIGINS_SECTION_NAME));

            Services = services;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<AllowedOriginsConstants> allowedOriginsOptions)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(option => option.WithOrigins(allowedOriginsOptions.Value.Origins)
                                        .AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowAnyOrigin());

            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/{SWAGGER_VERSION}/swagger.json", $"{SOLUTION_NAME} {SWAGGER_VERSION}"); });
        }
    }
}