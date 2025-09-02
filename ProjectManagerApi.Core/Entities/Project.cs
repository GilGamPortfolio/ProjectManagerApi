using System;
using System.Collections.Generic;

namespace ProjectManagerApi.Core.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid OwnerId { get; private set; } // Chave estrangeira para o proprietário do projeto

        // **********************************************
        // NOVO: Propriedades de Navegação
        // **********************************************
        // Propriedade de navegação para o proprietário do projeto (User)
        public User Owner { get; private set; }

        // Coleção de navegação para as tarefas associadas ao projeto (TaskItem)
        public ICollection<TaskItem> TaskItems { get; private set; } = new List<TaskItem>();

        // Coleção de navegação para os membros do projeto (Users) - se você tiver essa relação many-to-many
        // Para uma relação many-to-many, você pode precisar de uma tabela de junção explícita
        // ou o EF Core pode inferir, dependendo da sua configuração e como User.cs gerencia isso.
        // public ICollection<User> Members { get; private set; } = new List<User>();


        private Project() { } // Construtor privado para o EF Core

        public Project(string name, string description, Guid ownerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be null or empty.", nameof(name));
            if (ownerId == Guid.Empty)
                throw new ArgumentException("Owner ID cannot be an empty GUID.", nameof(ownerId));

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            OwnerId = ownerId;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("New project name cannot be null or empty.", nameof(newName));
            Name = newName;
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }

        public void UpdateOwner(Guid newOwnerId)
        {
            if (newOwnerId == Guid.Empty)
                throw new ArgumentException("New owner ID cannot be an empty GUID.", nameof(newOwnerId));
            OwnerId = newOwnerId;
        }
    }
}