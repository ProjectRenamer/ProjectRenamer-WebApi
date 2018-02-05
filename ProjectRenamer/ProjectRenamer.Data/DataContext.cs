using ProjectRenamer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProjectRenamer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions contextOptions) : base(contextOptions)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
    }
}