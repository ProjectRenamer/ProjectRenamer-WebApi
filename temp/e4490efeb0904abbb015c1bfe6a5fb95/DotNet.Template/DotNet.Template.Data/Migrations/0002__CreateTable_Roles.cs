using System;
using FluentMigrator;

namespace DotNet.Template.Data.Migrations
{
    [Migration(0002, "Adem Catamak")]
    public class _0002__CreateTable_Roles : Migration
    {
        public override void Up()
        {
            Create.Table(TableNames.Roles)
                      .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                      .WithColumn("RoleName").AsString(50).NotNullable()
                      .WithColumn("Explanation").AsString(100).Nullable()
                      .WithColumn("UpdatedBy").AsString(100).Nullable()
                      .WithColumn("UpdateDate").AsDateTime().WithDefaultValue(new DateTime()).NotNullable()
                      .WithColumn("CreatedBy").AsString(100).Nullable()
                      .WithColumn("CreateDate").AsDateTime().WithDefaultValue(new DateTime()).NotNullable()
                      .WithColumn("IsDeleted").AsBoolean().WithDefaultValue(false).NotNullable();

            Create.Index("IX_RoleName")
                  .OnTable(TableNames.Roles)
                  .OnColumn("RoleName")
                  .Unique();

            Insert.IntoTable(TableNames.Roles)
                  .InSchema("dbo")
                  .Row(new
                  {
                      Id = 1,
                      RoleName = "21min"
                  });
        }

        public override void Down()
        {
            Delete.Table(TableNames.Roles);
        }
    }
}
