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

            // Relação TaskItem -> Project (um TaskItem pertence a um Project)
            // Se um Project for deletado, os TaskItems associados a ele devem ser deletados em cascata.
            modelBuilder.Entity<TaskItem>()
                .HasOne<Project>()
                .WithMany()
                .HasForeignKey(t => t.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Manter Cascade aqui (TaskItem morre com o Project)

            // Relação Comment -> TaskItem (um Comment pertence a um TaskItem)
            // Se um TaskItem for deletado, os Comments associados a ele devem ser deletados em cascata.
            modelBuilder.Entity<Comment>()
                .HasOne<TaskItem>()
                .WithMany()
                .HasForeignKey(c => c.TaskItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade); // Manter Cascade aqui (Comment morre com o TaskItem)

            // Relação Project -> User (um Project tem um Owner - User)
            // *** ESTA É A MUDANÇA CRÍTICA PARA RESOLVER O ERRO MAIS RECENTE ***
            // DEVE SER DeleteBehavior.Restrict para evitar o ciclo de cascata com TaskItem.AssigneeId.
            // Isso significa que você NÃO poderá deletar um User se ele ainda for o owner de Projects.
            modelBuilder.Entity<Project>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // <--- MUDANÇA AQUI: DE CASCADE PARA RESTRICT

            // Relação TaskItem -> User (um TaskItem pode ser atribuído a um Assignee - User)
            // Manter SetNull, pois o conflito principal é resolvido acima.
            modelBuilder.Entity<TaskItem>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.AssigneeId)
                .IsRequired(false) // Permite AssigneeId ser NULL
                .OnDelete(DeleteBehavior.SetNull); // Manter SetNull aqui

            // Relação Comment -> User (um Comment foi feito por um User)
            // Manter Restrict, pois resolveu um problema anterior e é uma boa prática aqui.
            modelBuilder.Entity<Comment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // Manter Restrict aqui
        }
    }
}