using System;
using System.Collections.Generic;

namespace ProjectManagerApi.Core.Entities
{
    public class Comment
    {
        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public Guid TaskItemId { get; private set; } 
        public Guid UserId { get; private set; }

        public TaskItem TaskItem { get; private set; }

        public User User { get; private set; }

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

        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
                throw new ArgumentException("New comment content cannot be null or empty.", nameof(newContent));
            Content = newContent;
        }
    }
}