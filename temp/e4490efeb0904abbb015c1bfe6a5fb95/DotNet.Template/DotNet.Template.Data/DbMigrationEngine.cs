using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet.Template.Data
{
    public static class DbMigrationEngine
    {
        public enum DbOptions
        {
            Sqlite = 1,
            MySql = 2,
            MsSql = 3
        }

        public static void MigrateUp(DbOptions dbOptions, string connectionStrings)
        {
            IServiceProvider serviceProvider = CreateServices(dbOptions, connectionStrings);

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetService<IMigrationRunner>();

                runner.MigrateUp();
            }
        }

        public static void MigrateDown(DbOptions dbOptions, string connectionStrings, long toVersion)
        {
            IServiceProvider serviceProvider = CreateServices(dbOptions, connectionStrings);

            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                runner.MigrateDown(toVersion);
            }
        }

        private static IServiceProvider CreateServices(DbOptions dbOptions, string dbConnectionString)
        {
            switch (dbOptions)
            {
                case DbOptions.Sqlite:
                    return new ServiceCollection()
                           .AddFluentMigratorCore()
                           .ConfigureRunner(rb => rb
                                                  .AddSQLite()
                                                  .WithGlobalConnectionString(dbConnectionString)
                                                  .ScanIn(typeof(DbMigrationEngine).Assembly).For.Migrations())
                           .AddLogging(lb => lb.AddFluentMigratorConsole())
                           .BuildServiceProvider(false);
                case DbOptions.MySql:
                    return new ServiceCollection()
                           .AddFluentMigratorCore()
                           .ConfigureRunner(rb => rb
                                                  .AddMySql5()
                                                  .WithGlobalConnectionString(dbConnectionString)
                                                  .ScanIn(typeof(DataContext).Assembly).For.Migrations())
                           .AddLogging(lb => lb.AddFluentMigratorConsole())
                           .BuildServiceProvider(false);
                case DbOptions.MsSql:
                    return new ServiceCollection()
                           .AddFluentMigratorCore()
                           .ConfigureRunner(rb => rb
                                                  .AddSqlServer()
                                                  .WithGlobalConnectionString(dbConnectionString)
                                                  .ScanIn(typeof(DataContext).Assembly).For.Migrations())
                           .AddLogging(lb => lb.AddFluentMigratorConsole())
                           .BuildServiceProvider(false);
                default:
                    throw new ArgumentOutOfRangeException(nameof(dbOptions), dbOptions, null);
            }
        }
    }
}