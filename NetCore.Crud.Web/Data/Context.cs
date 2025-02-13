using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCore.Crud.Web.Models;

namespace NetCore.Crud.Web.Data
{
    public class Context : IdentityDbContext<User>
    {
        public Context(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<UserAssignment> UsersAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.ToTable("Assignment");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Code).HasMaxLength(100);
                entity.Property(a => a.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(a => a.Code).HasMaxLength(100);
                entity.Property(a => a.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<UserAssignment>(entity =>
            {
                entity.ToTable("UserAssignment");
                entity.HasKey(a => new { a.UserId, a.AssignmentId });
                entity.HasOne(a => a.User)
                    .WithMany(b => b.UsersAssignments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(a => a.Assignment)
                    .WithMany(b => b.UsersAssignments)
                    .HasForeignKey(c => c.AssignmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
