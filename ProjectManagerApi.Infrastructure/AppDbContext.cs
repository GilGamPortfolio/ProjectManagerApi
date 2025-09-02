using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity; // Certifique-se de ter este using

namespace ProjectManagerApi.Infrastructure
{
    // AQUI: 'User' é a sua entidade User personalizada, que agora herda de IdentityUser<Guid>.
    // 'IdentityRole<Guid>' indica que os IDs dos papéis também serão Guid.
    // 'Guid' é o tipo da chave primária para usuários e papéis.
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid> // <--- CORREÇÃO AQUI (se não for ApplicationUser)
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // REMOVIDO: DbSet<User> Users { get; set; }
        // O IdentityDbContext já gerencia o DbSet para a sua entidade 'User' (agora IdentityUser).

        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // CRÍTICO: Chamar o base.OnModelCreating(modelBuilder) ANTES das suas configurações.
            // Isso garante que as tabelas do Identity (AspNetUsers, AspNetRoles, etc.) sejam criadas corretamente.
            base.OnModelCreating(modelBuilder);

            // **********************************************
            // AJUSTES: Configurações de Relacionamento
            // **********************************************

            // Configuração da relação Project -> User (Owner)
            // Agora a propriedade Owner (Type User) em Project se refere à sua entidade User que é IdentityUser<Guid>
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Owner)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.OwnerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação TaskItem -> Project
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Project)
                .WithMany(p => p.TaskItems)
                .HasForeignKey(t => t.ProjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração da relação TaskItem -> User (Assignee)
            // Novamente, Assignee (Type User) se refere à sua entidade User que é IdentityUser<Guid>
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTasks) // Esta coleção deve existir na sua entidade User
                .HasForeignKey(t => t.AssigneeId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Configuração da relação Comment -> TaskItem
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.TaskItem)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Configuração da relação Comment -> User (Author)
            // O User aqui se refere à sua entidade User que é IdentityUser<Guid>
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments) // Esta coleção deve existir na sua entidade User
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}