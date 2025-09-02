// No arquivo TaskItem.cs

using System;
using ProjectManagerApi.Core.Enums;
using System.Collections.Generic; // Certifique-se de que este using está presente para ICollection e List

namespace ProjectManagerApi.Core.Entities
{
    public class TaskItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Enums.TaskStatus Status { get; private set; }
        public Enums.Priority Priority { get; private set; } // Esta propriedade está presente, não está faltando.

        // Chaves Estrangeiras
        public Guid ProjectId { get; private set; }
        public Guid? AssigneeId { get; private set; }

        // Propriedades de Navegação
        public Project Project { get; private set; }
        public User Assignee { get; private set; }
        public ICollection<Comment> Comments { get; private set; } = new List<Comment>();


        private TaskItem() { } // Construtor privado para o EF Core

        // CONSTRUTOR CORRIGIDO
        public TaskItem(string title, string description, Guid projectId, Guid? assigneeId, Enums.Priority priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title cannot be null or empty.", nameof(title));
            if (projectId == Guid.Empty)
                throw new ArgumentException("Project ID cannot be an empty GUID.", nameof(projectId));

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Status = Enums.TaskStatus.ToDo; // Estado inicial padrão
            Priority = priority;
            ProjectId = projectId;
            AssigneeId = assigneeId;
        }

        // MÉTODOS DE ATUALIZAÇÃO (mantidos como estão)
        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("New task title cannot be null or empty.", nameof(newTitle));
            Title = newTitle;
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }

        public void UpdateStatus(Enums.TaskStatus newStatus)
        {
            Status = newStatus;
        }

        public void UpdatePriority(Enums.Priority newPriority)
        {
            Priority = newPriority;
        }

        public void AssignToUser(Guid? userId)
        {
            AssigneeId = userId;
        }
    }
}