using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Application.DTOs
{
    public class UpdateProjectDto
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The project name must be between {2} and {1} characters.")]
        public string? Name { get; set; }

        [StringLength(500, ErrorMessage = "The description cannot exceed {1} characters.")]
        public string? Description { get; set; }

        public Guid? OwnerId { get; set; }
    }
}