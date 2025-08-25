// ProjectManagerApi.Application/Interfaces/ITaskItemRepository.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagerApi.Core.Entities; // Certifique-se de referenciar o projeto Core

namespace ProjectManagerApi.Application.Interfaces
{
    public interface ITaskItemRepository
    {
        Task<TaskItem> GetByIdAsync(Guid id);
        Task<List<TaskItem>> GetByProjectIdAsync(Guid projectId); // Para listar tarefas de um projeto específico
        Task AddAsync(TaskItem taskItem);
        Task UpdateAsync(TaskItem taskItem);
        Task DeleteAsync(TaskItem taskItem);
    }
}