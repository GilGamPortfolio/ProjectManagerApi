// ProjectManagerApi.Core/Entities/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic; // Para coleções, se necessário

namespace ProjectManagerApi.Core.Entities
{
    // A classe `User` que você já tem no Core representa a entidade de negócio.
    // Esta `ApplicationUser` será a entidade que o ASP.NET Core Identity gerencia para autenticação.
    // É comum ter uma separação ou mapeamento entre elas, mas para começar, vamos usar esta para o Identity.
    public class ApplicationUser : IdentityUser
    {
        // Adicione propriedades personalizadas aqui, se precisar de algo além de UserName, Email, PhoneNumber, etc.
        // Exemplo:
        // public string FullName { get; set; }
        // public DateTime DateOfBirth { get; set; }

        // Você pode ter um construtor se precisar de inicialização específica
        public ApplicationUser() : base() { }

        // Se precisar de um construtor com userName para o Identity
        public ApplicationUser(string userName) : base(userName) { }
    }
}