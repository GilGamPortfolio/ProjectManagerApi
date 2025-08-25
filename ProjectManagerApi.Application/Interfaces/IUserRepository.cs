// ProjectManagerApi.Application/Interfaces/IUserRepository.cs

using System;
using System.Threading.Tasks;
using ProjectManagerApi.Core.Entities; // Certifique-se de referenciar o projeto Core

namespace ProjectManagerApi.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email); // Para busca de usuário por e-mail, útil na autenticação
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        // Poderíamos adicionar métodos de listagem, paginação, etc., mas por agora, focamos no básico.
    }
}