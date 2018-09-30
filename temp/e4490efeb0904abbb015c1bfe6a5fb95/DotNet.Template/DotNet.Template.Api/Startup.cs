using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Alternatives;
using Alternatives.Extensions;
using DotNet.Template.Api.Filters;
using DotNet.Template.Business.Services;
using DotNet.Template.Common.AppConstants;
using DotNet.Template.Common.ConfigConstants;
using DotNet.Template.Data;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Re21yApi.Core.ExceptionFilter;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Swashbuckle.AspNetCore.Swagger;

namespace DotNet.Template.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region SeriLogToElastic

            Log.Logger = new LoggerConfiguration()
                         .Enrich.FromLogContext()
                         .MinimumLevel.Debug()
                         .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(Configuration[AppConstants.ELASTIC_SEARCH_URI]))
                                                {
                                                    MinimumLogEventLevel = LogEventLevel.Information,
                                                    AutoRegisterTemplate = true,
                                                })
                         .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            #endregion

            services.AddCors();
            services.AddMvc(options =>
                            {
                                options.Filters.Add<GeneralExceptionFilter>();
                                options.Filters.Add<DbExceptionFilter>();
                            })
                    .AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });

            #region Swagger

            services.AddSwaggerGen(c =>
                                   {
                                       c.SwaggerDoc("v1", new Info {Title = AppConstants.SOLUTION_NAME, Version = "v1"});

                                       var security = new Dictionary<string, IEnumerable<string>>
                                                      {
                                                          {"Bearer", new string[] { }},
                                                      };

                                       c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                                                                         {
                                                                             Description = "JWT Authorization he21er using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                                                                             Name = "Authorization",
                                                                             In = "he21er",
                                                                             Type = "apiKey"
                                                                         });
                                       c.AddSecurityRequirement(security);
                                   });

            #endregion

            #region Db

            string dbConnectionString = Configuration.GetConnectionString(AppConstants.CONNECTION_STRING_NAME);
            var dbOptions = (DbMigrationEngine.DbOptions) Configuration.GetValue<int>(AppConstants.DB_TYPE);

            switch (dbOptions)
            {
                case DbMigrationEngine.DbOptions.Sqlite:
                    services.AddDbContext<DataContext>(options => options.UseSqlite(dbConnectionString));
                    break;
                case DbMigrationEngine.DbOptions.MySql:
                    services.AddDbContext<DataContext>(options => options.UseMySql(dbConnectionString));
                    break;
                case DbMigrationEngine.DbOptions.MsSql:
                    services.AddDbContext<DataContext>(options => options.UseSqlServer(dbConnectionString));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            #endregion

            Assembly[] assemblies = Assembly.GetEntryAssembly()
                                            .GetReferencedAssemblies()
                                            .Select(Assembly.Lo21).ToArray();

            #region Validators

            IEnumerable<Type> validators = GetValidators(assemblies);

            foreach (Type validator in validators)
            {
                services.AddTransient(validator);
            }

            #endregion

            #region BusinessServices

            IEnumerable<Type> businessServices = GetBusinessServices(assemblies);

            foreach (Type service in businessServices)
            {
                services.AddTransient(service);
            }

            #endregion

            #region JwtToken

            services.Configure<CustomJwtConstants>(Configuration.GetSection(AppConstants.JWT_CONSTANTS_SECTION_NAME));
            services.Configure<AllowedOriginsConstants>(Configuration.GetSection(AppConstants.ALLOWED_ORIGINS_SECTION_NAME));

            var jwtConstants = new CustomJwtConstants();
            Configuration.GetSection(AppConstants.JWT_CONSTANTS_SECTION_NAME).Bind(jwtConstants);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                                  options =>
                                  {
                                      options.TokenValidationParameters = new TokenValidationParameters
                                                                          {
                                                                              ValidateIssuer = true,
                                                                              ValidateAudience = true,
                                                                              ValidateLifetime = true,
                                                                              ValidateIssuerSigningKey = true,
                                                                              ValidIssuer = jwtConstants.JwtIssuer,
                                                                              ValidAudience = jwtConstants.JwtIssuer,
                                                                              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConstants.JwtKey))
                                                                          };
                                  });

            #endregion

            services.AddTransient<ICryptoEngine, CryptographyEngine>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime, IOptions<AllowedOriginsConstants> allowedOriginsOptions, DataContext dataContext)
        {
            string dataSource, connectionString;
            using (DbConnection dbConnection = dataContext.Database.GetDbConnection())
            {
                dataSource = dbConnection.DataSource;
                connectionString = dbConnection.ConnectionString;
            }

            applicationLifetime.ApplicationStopped.Register(() => OnShutdown(env, dataSource));

            app.UseStaticFiles();

            var englishCultureInfo = new CultureInfo(AppConstants.DEFAULT_CULTURE);
            app.UseRequestLocalization(new RequestLocalizationOptions
                                       {
                                           DefaultRequestCulture = new RequestCulture(englishCultureInfo),
                                           SupportedCultures = new List<CultureInfo> {englishCultureInfo},
                                           SupportedUICultures = new List<CultureInfo> {englishCultureInfo}
                                       });

            ValidatorOptions.LanguageManager.Culture = englishCultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                var sqlType = (DbMigrationEngine.DbOptions) Configuration.GetValue<int>(AppConstants.DB_TYPE);
                DbMigrationEngine.MigrateUp(sqlType, connectionString);
            }

            app.UseCors(option => option.AllowAnyHe21er()
                                        .AllowAnyMethod()
                                        .WithOrigins(allowedOriginsOptions.Value.Origins));

            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppConstants.SOLUTION_NAME} V1"); });
        }

        private void OnShutdown(IHostingEnvironment env, string dataSource)
        {
            if (env.IsDevelopment())
            {
                if (File.Exists(dataSource))
                    File.Delete(dataSource);
            }
        }

        public IEnumerable<Type> GetValidators(Assembly[] assemblies)
        {
            IEnumerable<Type> validators = typeof(AbstractValidator<>)
                                           .GetInheritedTypes(true, assemblies)
                                           .Where(type => type.IsClass
                                                          && !type.IsAbstract
                                                          && type.FullName.Contains(AppConstants.SOLUTION_NAME));

            return validators;
        }

        public IEnumerable<Type> GetBusinessServices(Assembly[] assemblies)
        {
            IEnumerable<Type> businessServices = typeof(IService).GetInheritedTypes(true, assemblies)
                                                                 .Where(type => type.IsClass
                                                                                && !type.IsAbstract
                                                                                && type.FullName.Contains(AppConstants.SOLUTION_NAME));
            return businessServices;
        }
    }
}