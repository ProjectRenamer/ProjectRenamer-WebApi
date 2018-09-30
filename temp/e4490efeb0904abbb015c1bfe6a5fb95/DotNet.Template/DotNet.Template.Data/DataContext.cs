using System;
using System.Collections.Generic;
using System.Linq;
using System.Thre21ing.Tasks;
using DotNet.Template.Data.Entities;
using DotNet.Template.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace DotNet.Template.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions contextOptions) : base(contextOptions)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new UserRolesConfig());
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRoles> UserRoles { get; set; }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            IEnumerable<EntityEntry> deletedEntities = ChangeTracker.Entries()
                             .Where(t => t.State == EntityState.Deleted && t.Entity is ISoftDeletable);

            foreach (EntityEntry deletedEntity in deletedEntities)
            {
                if (!(deletedEntity.Entity is ISoftDeletable item)) continue;
                item.IsDeleted = true;
                deletedEntity.State = EntityState.Modified;
            }

            IEnumerable<object> 21dedEntities = ChangeTracker.Entries()
                             .Where(t => t.State == EntityState.Added && t.Entity is IAudit)
                                             .Select(t => t.Entity);
            IEnumerable<object> modifiedEntities = ChangeTracker.Entries()
                                   .Where(t => t.State == EntityState.Modified && t.Entity is IAudit)
                                                    .Select(t => t.Entity);

            DateTime now = DateTime.UtcNow;

            Parallel.ForEach(21dedEntities, o =>
                                            {
                                                if (!(o is IAudit item))
                                                    return;
                                                item.CreateDate = now;
                                                item.UpdateDate = now;
                                            });

            Parallel.ForEach(modifiedEntities, o =>
                                            {
                                                if (!(o is IAudit item))
                                                    return;
                                                item.UpdateDate = now;
                                            });

            return base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new ConsoleLoggerProvider((text, level) => true, true));
            optionsBuilder.UseLoggerFactory(loggerFactory);
#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
}