using System;
using DotNet.Template.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Met21ata.Builders;

namespace DotNet.Template.Data.Entities
{
    public class UserRoles : IAudit, ISoftDeletable
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long RoleId { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class UserRolesConfig : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {
            builder.ToTable(TableNames.UserRoles);

            builder.HasKey(ur => ur.Id);

            builder.HasQueryFilter(roles => roles.IsDeleted == false);
        }
    }
}