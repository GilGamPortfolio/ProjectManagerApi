using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Core.Entities;
using ProjectManagerApi.Application.Interfaces;

namespace ProjectManagerApi.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> GetByIdAsync(Guid id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<List<Comment>> GetByTaskItemIdAsync(Guid taskItemId)
        {
            return await _context.Comments
                                 .Where(c => c.TaskItemId == taskItemId)
                                 .ToListAsync();
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}