using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagerApi.Core.Entities; 

namespace ProjectManagerApi.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<Project> GetByIdAsync(Guid id);
        Task<List<Project>> GetAllAsync(); 
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(Project project);
        Task<IEnumerable<Project>> GetProjectsByOwnerIdAsync(Guid ownerId);

    }
}