// ProjectManagerApi.Application/Interfaces/ICommentRepository.cs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManagerApi.Core.Entities; // Certifique-se de referenciar o projeto Core

namespace ProjectManagerApi.Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetByIdAsync(Guid id);
        Task<List<Comment>> GetByTaskItemIdAsync(Guid taskItemId); // Para listar comentários de uma tarefa específica
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Comment comment);
    }
}