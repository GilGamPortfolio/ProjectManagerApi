// ProjectManagerApi.API/Controllers/SecureController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ProjectManagerApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecureController : ControllerBase
    {
        [HttpGet("data")]
        [Authorize] // Este endpoint requer autenticação
        public IActionResult GetData()
        {
            // Acessa informações do usuário autenticado via User
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            return Ok($"Este é um endpoint protegido. Olá, {userName} (ID: {userId}, Email: {userEmail})!");
        }

        [HttpGet("admin-data")]
        [Authorize(Roles = "Admin")] // Este endpoint requer autenticação E o papel "Admin"
        public IActionResult GetAdminData()
        {
            return Ok("Este é um endpoint super secreto, apenas para administradores!");
        }
    }
}