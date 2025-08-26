using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Application.DTOs
{
    public class UpdateCommentDto
    {
        [Required(ErrorMessage = "New comment content is required for update.")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Comment content must be between {2} and {1} characters.")]
        public string Content { get; set; }
    }
}