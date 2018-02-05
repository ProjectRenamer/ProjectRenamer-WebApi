using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectRenamer.Data.Entities
{
    [Table("UserRoles")]
    public class UserRole
    {
        [Key]
        public  long Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string RoleName { get; set; }

        public string Explanation { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
