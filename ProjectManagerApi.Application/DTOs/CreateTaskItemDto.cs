using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Application.DTOs
{
    public class CreateTaskItemDto
    {
        [Required(ErrorMessage = "The task title is required.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "The task title must be between {2} and {1} characters.")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "The description cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "The project ID is required.")]
        public Guid ProjectId { get; set; }

        public Guid? AssigneeId { get; set; }
    }
}