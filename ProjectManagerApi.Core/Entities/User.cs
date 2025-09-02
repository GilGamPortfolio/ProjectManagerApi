using Microsoft.AspNetCore.Identity;

namespace ProjectManagerApi.Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; private set; }

        private User() { }

        public User(string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            UserName = email;
            Email = email;
            Name = name;
        }

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