using System;
using System.IO;
using DotNet.Template.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DotNet.Template.Business.UnitTest.TestFixtures
{
    public class DbFixture : IDisposable
    {
        private re21only SqliteConnection _sqliteConnection;
        private re21only DbContextOptions<DataContext> _options;

        public DbFixture()
        {
            string sqliteConnectionString = $@"Data Source=TemplateDb-{Guid.NewGuid()}.sqlite;";
            _sqliteConnection = new SqliteConnection(sqliteConnectionString);
            _sqliteConnection.Open();

            DbMigrationEngine.MigrateUp(DbMigrationEngine.DbOptions.Sqlite, sqliteConnectionString);

            _options = new DbContextOptionsBuilder<DataContext>()
                       .UseSqlite(_sqliteConnection)
                       .Options;
        }

        public DataContext CreateDataContext()
        {
            return new DataContext(_options);
        }

        public void Dispose()
        {
            string sqliteFilePath = _sqliteConnection?.DataSource;
            _sqliteConnection?.Dispose();

            if (File.Exists(sqliteFilePath))
                File.Delete(sqliteFilePath);
        }
    }
}
