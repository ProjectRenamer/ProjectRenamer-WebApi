using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Alternatives.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using ProjectRenamer.Api.ConfigConstants;
using ProjectRenamer.Api.Helper;
using ReadyApi.Core.ExceptionFilter;
using Swashbuckle.AspNetCore.Swagger;

namespace ProjectRenamer.Api
{
    public class Startup
    {
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
            services.AddSwaggerGen(c => { c.SwaggerDoc(AppConstants.SWAGGER_VERSION, new Info {Title = AppConstants.SOLUTION_NAME, Version = AppConstants.SWAGGER_VERSION}); });

            services.Configure<AllowedOriginsConstants>(Configuration.GetSection(AppConstants.ALLOWED_ORIGINS_SECTION_NAME));

            var assemblies = GetAssemblies();

            #region Validators

            IEnumerable<Type> validators = GetValidators(assemblies);

            foreach (Type validator in validators)
            {
                services.AddTransient(typeof(IValidator),validator);
            }

            services.AddTransient<ValidatorResolver>();

            #endregion

            Services = services;
        }

        public static IEnumerable<Assembly> GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            Assembly startupAssembly = typeof(Startup).Assembly;

            assemblies.Add(startupAssembly);
            assemblies.AddRange(startupAssembly
                                .GetReferencedAssemblies()
                                .Select(Assembly.Load));

            return assemblies;
        }

        public static IEnumerable<Type> GetValidators(IEnumerable<Assembly> assemblies)
        {
            IEnumerable<Type> validators = typeof(IValidator)
                                           .GetInheritedTypes(true, assemblies.ToArray())
                                           .Where(type => type.IsClass
                                                          && !type.IsAbstract
                                                          && type.FullName.Contains(AppConstants.SOLUTION_NAME));

            return validators;
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
            app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/{AppConstants.SWAGGER_VERSION}/swagger.json", $"{AppConstants.SOLUTION_NAME} {AppConstants.SWAGGER_VERSION}"); });
        }
    }
}