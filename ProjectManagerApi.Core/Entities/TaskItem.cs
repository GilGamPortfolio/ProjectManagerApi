// ProjectManagerApi.Core/Entities/TaskItem.cs
using System;
using ProjectManagerApi.Core.Enums;
using TaskStatus = ProjectManagerApi.Core.Enums.TaskStatus;

namespace ProjectManagerApi.Core.Entities
{
    public class TaskItem
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public TaskStatus Status { get; private set; } // Using the enum
        public Guid ProjectId { get; private set; } // The ID of the Project this task belongs to
        public Guid? AssigneeId { get; private set; } // The ID of the User assigned to this task (nullable)

        // Private constructor for EF Core and other internal uses
        private TaskItem() { }

        public TaskItem(string title, string description, Guid projectId, Guid? assigneeId = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title cannot be null or empty.", nameof(title));
            if (projectId == Guid.Empty)
                throw new ArgumentException("Project ID cannot be an empty GUID.", nameof(projectId));

            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Status = TaskStatus.ToDo; // Default status when created
            ProjectId = projectId;
            AssigneeId = assigneeId;
        }

        // Methods to update properties
        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("New task title cannot be null or empty.", nameof(newTitle));
            Title = newTitle;
        }

        public void UpdateDescription(string newDescription)
        {
            // Description can be empty or null
            Description = newDescription;
        }

        public void UpdateStatus(TaskStatus newStatus)
        {
            Status = newStatus;
        }

        public void AssignToUser(Guid? userId)
        {
            // AssigneeId can be null if unassigned
            AssigneeId = userId;
        }
    }
}