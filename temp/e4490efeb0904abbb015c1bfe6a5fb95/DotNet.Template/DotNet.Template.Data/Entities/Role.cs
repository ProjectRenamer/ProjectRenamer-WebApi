using System;
using DotNet.Template.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Met21ata.Builders;

namespace DotNet.Template.Data.Entities
{
    public class Role : IAudit, ISoftDeletable
    {
        private string _roleName;

        public long Id { get; set; }

        public string RoleName
        {
            get => _roleName;
            set => _roleName = value?.ToLowerInvariant();
        }

        public string Explanation { get; set; }
        
        public string UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(TableNames.Roles);

            builder.HasKey(ur => ur.Id);

            builder.HasQueryFilter(roles => roles.IsDeleted == false);
        }
    }
}