// ProjectManagerApi.Infrastructure/AppDbContext.cs

using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Core.Entities; // Certifique-se de adicionar esta referência

namespace ProjectManagerApi.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets para suas entidades
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração dos relacionamentos usando Fluent API
            // Cada entidade 'filha' possui uma chave estrangeira para a entidade 'pai'

            // Relacionamento entre Project e TaskItem (Um projeto tem muitas tarefas)
            modelBuilder.Entity<TaskItem>()
                .HasOne<Project>() // Uma TaskItem tem um Project
                .WithMany()        // Um Project tem muitas TaskItems (sem propriedade de navegação direta no Project por enquanto)
                .HasForeignKey(t => t.ProjectId) // A chave estrangeira está em TaskItem
                .IsRequired();     // Uma TaskItem sempre deve ter um Project associado

            // Relacionamento entre TaskItem e Comment (Uma tarefa tem muitos comentários)
            modelBuilder.Entity<Comment>()
                .HasOne<TaskItem>() // Um Comment tem uma TaskItem
                .WithMany()         // Uma TaskItem tem muitos Comments
                .HasForeignKey(c => c.TaskItemId) // A chave estrangeira está em Comment
                .IsRequired();      // Um Comment sempre deve ter uma TaskItem associada

            // Relacionamento entre User e Project (Um usuário pode ser dono de muitos projetos)
            modelBuilder.Entity<Project>()
                .HasOne<User>()      // Um Project tem um User como Owner
                .WithMany()          // Um User pode ter muitos Projects (como Owner)
                .HasForeignKey(p => p.OwnerId) // A chave estrangeira está em Project
                .IsRequired();       // Um Project sempre deve ter um Owner

            // Relacionamento entre User e TaskItem (Um usuário pode ser atribuído a muitas tarefas - AssigneeId)
            modelBuilder.Entity<TaskItem>()
                .HasOne<User>()      // Uma TaskItem pode ter um User como Assignee
                .WithMany()          // Um User pode ser Assignee de muitas TaskItems
                .HasForeignKey(t => t.AssigneeId) // A chave estrangeira está em TaskItem
                .IsRequired(false);  // AssigneeId é anulável (Guid?)

            // Relacionamento entre User e Comment (Um usuário pode fazer muitos comentários)
            modelBuilder.Entity<Comment>()
                .HasOne<User>()      // Um Comment tem um User como autor
                .WithMany()          // Um User pode fazer muitos Comments
                .HasForeignKey(c => c.UserId) // A chave estrangeira está em Comment
                .IsRequired();       // Um Comment sempre deve ter um User associado
        }
    }
}