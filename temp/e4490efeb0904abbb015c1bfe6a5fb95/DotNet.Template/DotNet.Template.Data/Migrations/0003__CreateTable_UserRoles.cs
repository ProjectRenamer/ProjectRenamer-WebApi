using System;
using FluentMigrator;

namespace DotNet.Template.Data.Migrations
{
    [Migration(0003, "Adem Catamak")]
    public class _0003__CreateTable_UserRoles : Migration
    {
        public override void Up()
        {
            Create.Table(TableNames.UserRoles)
                      .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                      .WithColumn("UserId").AsInt64().NotNullable()
                      .WithColumn("RoleId").AsInt64().NotNullable()
                      .WithColumn("UpdatedBy").AsString(100).Nullable()
                      .WithColumn("UpdateDate").AsDateTime().WithDefaultValue(new DateTime()).NotNullable()
                      .WithColumn("CreatedBy").AsString(100).Nullable()
                      .WithColumn("CreateDate").AsDateTime().WithDefaultValue(new DateTime()).NotNullable()
                      .WithColumn("IsDeleted").AsBoolean().WithDefaultValue(false).NotNullable();

            Create.Index("IX_UserId_RoleId")
                  .OnTable(TableNames.UserRoles)
                  .OnColumn("UserId").Ascending()
                  .OnColumn("RoleId").Ascending()
                  .WithOptions()
                  .Unique();

            Insert.IntoTable(TableNames.UserRoles)
                  .InSchema("dbo")
                  .Row(new
                  {
                      UserId = 1,
                      RoleId = 1
                  });
        }

        public override void Down()
        {
            Delete.Table(TableNames.UserRoles);
        }
    }
}
