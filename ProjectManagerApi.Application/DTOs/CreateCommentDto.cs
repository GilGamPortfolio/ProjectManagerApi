using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Application.DTOs
{
    public class CreateCommentDto
    {
        [Required(ErrorMessage = "Comment content is required.")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Comment content must be between {2} and {1} characters.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Task Item ID is required.")]
        public Guid TaskItemId { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public Guid UserId { get; set; }
    }
}