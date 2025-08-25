using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectManagerApi.Core.Entities;
using ProjectManagerApi.Application.Interfaces;

namespace ProjectManagerApi.Infrastructure.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly AppDbContext _context;

        public TaskItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> GetByIdAsync(Guid id)
        {
            return await _context.TaskItems.FindAsync(id);
        }

        public async Task<List<TaskItem>> GetByProjectIdAsync(Guid projectId)
        {
            return await _context.TaskItems
                                 .Where(t => t.ProjectId == projectId)
                                 .ToListAsync();
        }

        public async Task AddAsync(TaskItem taskItem)
        {
            await _context.TaskItems.AddAsync(taskItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            _context.TaskItems.Update(taskItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskItem taskItem)
        {
            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();
        }
    }
}