using System;
using ProjectManagerApi.Core.Enums;
using TaskStatus = ProjectManagerApi.Core.Enums.TaskStatus;

namespace ProjectManagerApi.Application.DTOs
{
    public class TaskItemResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? AssigneeId { get; set; }
    }
}