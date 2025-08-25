using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagerApi.Core.Entities;

namespace ProjectManagerApi.Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetByIdAsync(Guid id);
        Task<List<Comment>> GetByTaskItemIdAsync(Guid taskItemId);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Comment comment);
    }
}