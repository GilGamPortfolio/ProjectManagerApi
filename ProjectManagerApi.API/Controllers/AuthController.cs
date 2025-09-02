// ProjectManagerApi.API/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using ProjectManagerApi.Core.Entities; // Sua ApplicationUser
using ProjectManagerApi.Application.DTOs.Auth; // Seus DTOs de Auth

namespace ProjectManagerApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User(model.UserName, model.Email); // Use seu construtor
            user.UserName = model.UserName; // Certifique-se que o UserName de IdentityUser seja setado
            user.Email = model.Email; // Certifique-se que o Email de IdentityUser seja setado

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Opcional: Adicionar usuário a um papel padrão, se houver
                // await _userManager.AddToRoleAsync(user, "User");
                return Ok(new { message = "Usuário registrado com sucesso!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }

            return Unauthorized("Credenciais inválidas.");
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!); // '!' para indicar que não será nulo em runtime

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // ID do usuário do Identity
                new Claim(ClaimTypes.Name, user.UserName!),    // Nome de usuário
                new Claim(ClaimTypes.Email, user.Email!),      // Email do usuário
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // JWT ID
            };

            // Se você usar papéis, adicione-os aos claims
            // var roles = await _userManager.GetRolesAsync(user);
            // foreach (var role in roles)
            // {
            //     claims.Add(new Claim(ClaimTypes.Role, role));
            // }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Token válido por 1 hora
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                // Opcional: Issuer e Audience (se ValidateIssuer/ValidateAudience estiverem true)
                // Issuer = jwtSettings["Issuer"],
                // Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}