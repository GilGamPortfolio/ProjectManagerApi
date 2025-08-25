
using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Core.Entities;

namespace ProjectManagerApi.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>()
                .HasOne<Project>() 
                .WithMany()
                .HasForeignKey(t => t.ProjectId)
                .IsRequired();

            modelBuilder.Entity<Comment>()
                .HasOne<TaskItem>()
                .WithMany()
                .HasForeignKey(c => c.TaskItemId)
                .IsRequired(); 

            modelBuilder.Entity<Project>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .IsRequired();

            modelBuilder.Entity<TaskItem>()
                .HasOne<User>()
                .WithMany() 
                .HasForeignKey(t => t.AssigneeId)
                .IsRequired(false);

            modelBuilder.Entity<Comment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .IsRequired();
        }
    }
}