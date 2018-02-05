using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectRenamer.Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(25)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(25)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; }

        [EmailAddress]
        [MaxLength(25)]
        public string OldEmail { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}