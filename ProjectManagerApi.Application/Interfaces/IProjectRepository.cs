// ProjectManagerApi.Application/Interfaces/IProjectRepository.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagerApi.Core.Entities; // Certifique-se de referenciar o projeto Core

namespace ProjectManagerApi.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(Guid id);
        Task<List<Project>> GetAllAsync(); // Para listar todos os projetos
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
    }
}