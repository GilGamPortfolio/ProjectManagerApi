using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Application.DTOs
{
    public class UpdateUserDto
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The name must be between {2} and {1} characters.")]
        public string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "The email cannot exceed {1} characters.")]
        public string? Email { get; set; }

        [MinLength(6, ErrorMessage = "The password must have at least {1} characters.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}