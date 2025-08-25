using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagerApi.Core.Entities; 

namespace ProjectManagerApi.Application.Interfaces
{
    public interface ITaskItemRepository
    {
        Task<TaskItem> GetByIdAsync(Guid id);
        Task<List<TaskItem>> GetByProjectIdAsync(Guid projectId);
        Task AddAsync(TaskItem taskItem);
        Task UpdateAsync(TaskItem taskItem);
        Task DeleteAsync(TaskItem taskItem);
    }
}