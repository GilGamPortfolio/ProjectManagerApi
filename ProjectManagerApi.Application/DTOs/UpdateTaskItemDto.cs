using System;
using System.ComponentModel.DataAnnotations;
using ProjectManagerApi.Core.Enums;
using TaskStatus = ProjectManagerApi.Core.Enums.TaskStatus;
namespace ProjectManagerApi.Application.DTOs
{
    public class UpdateTaskItemDto
    {
        [StringLength(150, MinimumLength = 3, ErrorMessage = "The task title must be between {2} and {1} characters.")]
        public string? Title { get; set; }

        [StringLength(1000, ErrorMessage = "The description cannot exceed {1} characters.")]
        public string? Description { get; set; }

        public TaskStatus? Status { get; set; }

        public Guid? AssigneeId { get; set; }
    }
}