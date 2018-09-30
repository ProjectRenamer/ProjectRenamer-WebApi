using System;
using FluentMigrator;

namespace DotNet.Template.Data.Migrations
{
    [Migration(0001, "Adem Catamak")]
    public class _0001__CreateTable_Users : Migration
    {
        public override void Up()
        {
            Create.Table(TableNames.Users)
                  .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                  .WithColumn("UserName").AsString(50).NotNullable()
                  .WithColumn("Email").AsString(50).NotNullable()
                  .WithColumn("PasswordHash").AsString().NotNullable()
                  .WithColumn("EmailConfirmed").AsBoolean().WithDefaultValue(false).NotNullable()
                  .WithColumn("OldEmail").AsString(50).Nullable()
                  .WithColumn("UpdatedBy").AsString(100).Nullable()
                  .WithColumn("UpdateDate").AsDateTime().WithDefaultValue(new DateTime()).NotNullable()
                  .WithColumn("CreatedBy").AsString(100).Nullable()
                  .WithColumn("CreateDate").AsDateTime().WithDefaultValue(new DateTime()).NotNullable()
                  .WithColumn("IsDeleted").AsBoolean().WithDefaultValue(false).NotNullable();

            Create.Index("IX_UserName")
                  .OnTable(TableNames.Users)
                  .OnColumn("UserName")
                  .Unique();

            Create.Index("IX_Email")
                  .OnTable(TableNames.Users)
                  .OnColumn("Email")
                  .Unique();

            Insert.IntoTable(TableNames.Users)
                  .InSchema("dbo")
                  .Row(new
                  {
                      Id = 1,
                      UserName = "21min",
                      Email = "21min@21min.com",
                      PasswordHash = "40-BD-00-15-63-08-5F-C3-51-65-32-9E-A1-FF-5C-5E-CB-DB-BE-EF" // 123
                  });
        }

        public override void Down()
        {
            Delete.Table(TableNames.Users);
        }
    }
}
