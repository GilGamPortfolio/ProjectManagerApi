using System;
using System.Collections.Generic; // Certifique-se de que este using esteja aqui se precisar de coleções no futuro

namespace ProjectManagerApi.Core.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public Guid TaskItemId { get; private set; } // Chave estrangeira para a TaskItem à qual o comentário pertence
        public Guid UserId { get; private set; } // Chave estrangeira para o Usuário que fez o comentário

        // **********************************************
        // NOVO: Propriedades de Navegação
        // **********************************************
        // Propriedade de navegação para a TaskItem associada
        public TaskItem TaskItem { get; private set; }

        // Propriedade de navegação para o Usuário autor do comentário
        public User User { get; private set; }

        private Comment() { } // Construtor privado para o EF Core

        public Comment(string content, Guid taskItemId, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Comment content cannot be null or empty.", nameof(content));
            if (taskItemId == Guid.Empty)
                throw new ArgumentException("Task Item ID cannot be an empty GUID.", nameof(taskItemId));
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be an empty GUID.", nameof(userId));

            Id = Guid.NewGuid();
            Content = content;
            TaskItemId = taskItemId;
            UserId = userId;
        }

        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("New comment content cannot be null or empty.", nameof(newContent));
            Content = newContent;
        }
    }
}