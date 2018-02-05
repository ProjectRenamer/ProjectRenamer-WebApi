using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectRenamer.Data.Entities
{
    [Table("UserClaims")]
    public class UserClaim
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public long UserRoleId { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
    }
}