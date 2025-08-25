using System;
using System.Collections.Generic;

namespace ProjectManagerApi.Core.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }

        private User() { }

        public User(string name, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash cannot be null or empty.", nameof(passwordHash));

            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("New name cannot be null or empty.", nameof(newName));
            Name = newName;
        }

        public void UpdatePasswordHash(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("New password hash cannot be null or empty.", nameof(newPasswordHash));
            PasswordHash = newPasswordHash;
        }
    }
}