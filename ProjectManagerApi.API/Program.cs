using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Infrastructure;
using ProjectManagerApi.Infrastructure.Repositories;
using ProjectManagerApi.Application.Interfaces;
using ProjectManagerApi.Application.Mappings;
using Microsoft.AspNetCore.Identity;
using ProjectManagerApi.Core.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura��o do ASP.NET Core Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    // Opcional: Configura��es de senha (para desenvolvimento, podemos ser mais flex�veis)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Opcional: Configura��es de usu�rio (ex: Email como nome de usu�rio)
    options.User.RequireUniqueEmail = true;

})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Configura��o da Autentica��o JWT Bearer

// CORRE��O AQUI: Garante que a chave JWT seja carregada ou lance uma exce��o clara
var jwtSecret = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key 'Jwt:Key' not found in configuration.");
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Apenas para desenvolvimento (em produ��o, sempre true)
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // Em produ��o, configure para o seu Issuer (URL da API)
        ValidateAudience = false, // Em produ��o, configure para o seu Audience (clientes da API)
        ValidateLifetime = true, // Valida a data de expira��o do token
        ClockSkew = TimeSpan.Zero // Sem toler�ncia de tempo no vencimento do token
    };
});

// servi�os existentes
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();