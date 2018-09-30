using System;
using System.IO;
using System.Reflection;
using DotNet.Template.Common.AppConstants;
using DotNet.Template.Data;
using Microsoft.Extensions.Configuration;

namespace DotNet.Template.Migrator
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Console.WriteLine("ASPNETCORE_ENVIRONMENT = " + environmentName);
            Console.WriteLine("Current Directory = " + currentDirectory);

            IConfigurationBuilder builder = new ConfigurationBuilder()
                                            .AddCommandLine(args)
                                            .SetBasePath(currentDirectory)
                                            .AddJsonFile("sharedsettings.json");

            if (!string.IsNullOrEmpty(environmentName))
            {
                builder.AddJsonFile($"sharedsettings.{environmentName}.json");
            }
            builder.AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();
            var sqlType = configuration.GetValue<int>(AppConstants.DB_TYPE);
            string dbConnectionString = configuration.GetConnectionString(AppConstants.CONNECTION_STRING_NAME);

            var limit = dbConnectionString.Length / 10;
            Console.WriteLine("Sql Type Id : " + sqlType);
            Console.WriteLine("Conn String : " + dbConnectionString.Substring(0, limit) + "..." + dbConnectionString.Substring(dbConnectionString.Length - (limit + 1), limit));

            DbMigrationEngine.MigrateUp((DbMigrationEngine.DbOptions)sqlType, dbConnectionString);
        }
    }
}
