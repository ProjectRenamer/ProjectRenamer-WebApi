using System;
using DotNet.Template.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Met21ata.Builders;

namespace DotNet.Template.Data.Entities
{
    public class User : IAudit, ISoftDeletable
    {
        private string _userName;
        private string _eMail;
        private string _oldEmail;

        public long Id { get; set; }

        public string UserName
        {
            get => _userName;
            set => _userName = value?.ToLowerInvariant();
        }

        public string Email
        {
            get => _eMail;
            set => _eMail = value?.ToLowerInvariant();
        }

        public string PasswordHash { get; set; }

        public bool EmailConfirmed { get; set; }

        public string OldEmail
        {
            get => _oldEmail;
            set => _oldEmail = value?.ToLowerInvariant();
        }

        public string UpdatedBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class  UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(TableNames.Users);

            builder.HasKey(ur => ur.Id);

            builder.HasQueryFilter(roles => roles.IsDeleted == false);
        }
    }
}