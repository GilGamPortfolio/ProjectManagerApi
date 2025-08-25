// ProjectManagerApi.Core/Entities/Comment.cs
using System;

namespace ProjectManagerApi.Core.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public Guid TaskItemId { get; private set; } // The ID of the TaskItem this comment belongs to
        public Guid UserId { get; private set; } // The ID of the User who made this comment

        // Private constructor for EF Core and other internal uses
        private Comment() { }

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

        // Method to update content (e.g., for editing a comment)
        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("New comment content cannot be null or empty.", nameof(newContent));
            Content = newContent;
        }
    }
}