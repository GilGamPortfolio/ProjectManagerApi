using System;
using System.Collections.Generic;

namespace ProjectManagerApi.Core.Entities
{
    public class Project
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid OwnerId { get; private set; }

        public User Owner { get; private set; }

        public ICollection<TaskItem> TaskItems { get; private set; } = new List<TaskItem>();
          
        private Project() { }

        public Project(string name, string description, Guid ownerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name cannot be null or empty.", nameof(name));
            if (ownerId == Guid.Empty)
                throw new ArgumentException("Owner ID cannot be an empty GUID.", nameof(ownerId));

            Id = Guid.NewGuid();
            Name = name;
            Description = description ?? string.Empty;
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
            Description = newDescription ?? string.Empty;
        }

        public void UpdateOwner(Guid newOwnerId)
        {
            if (newOwnerId == Guid.Empty)
                throw new ArgumentException("New owner ID cannot be an empty GUID.", nameof(newOwnerId));
            OwnerId = newOwnerId;
        }
    }
}