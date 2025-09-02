using Microsoft.AspNetCore.Identity;  
using System;
using System.Collections.Generic;

namespace ProjectManagerApi.Core.Entities
{
    // A classe User agora herda de IdentityUser<Guid> para usar Guid como tipo do ID
    public class User : IdentityUser<Guid>
    {
        // O Id, Email e PasswordHash são agora gerenciados por IdentityUser.
        // Você pode manter o 'Name' como uma propriedade customizada.

        // O Name é sua propriedade customizada que não é padrão do IdentityUser
        public string Name { get; private set; }

        // Construtor privado para EF Core (se necessário, mas IdentityUser já possui construtores)
        private User() { }

        // Construtor para criar um novo usuário com as propriedades customizadas
        public User(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            UserName = email; // Geralmente o UserName é o email para login
            Email = email;
            Name = name;
            // PasswordHash será definido pelo Identity Manager posteriormente (ex: UserManager.CreateAsync)
        }

        // Método para atualizar o nome customizado
        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("New name cannot be null or empty.", nameof(newName));
            Name = newName;
        }

        public ICollection<Project> Projects { get; private set; } = new List<Project>();
        public ICollection<TaskItem> AssignedTasks { get; private set; } = new List<TaskItem>();
        public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
    }
}