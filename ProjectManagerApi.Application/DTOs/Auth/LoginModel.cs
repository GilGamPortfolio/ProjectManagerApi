// ProjectManagerApi.Application/DTOs/Auth/LoginModel.cs
using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Application.DTOs.Auth
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; } // Ou Email, dependendo da sua preferência para login

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}