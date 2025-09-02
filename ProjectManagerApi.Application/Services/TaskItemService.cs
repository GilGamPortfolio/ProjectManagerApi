using AutoMapper;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Application.Interfaces;
using ProjectManagerApi.Application.Services.Interfaces; 
using ProjectManagerApi.Core.Entities;
using ProjectManagerApi.Core.Enums;

namespace ProjectManagerApi.Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IMapper _mapper;

        public TaskItemService(ITaskItemRepository taskItemRepository, IMapper mapper)
        {
            _taskItemRepository = taskItemRepository;
            _mapper = mapper;
        }

        public async Task<TaskItemResponseDto> CreateTaskItemAsync(CreateTaskItemDto taskItemDto)
        {

            var taskItem = new TaskItem(
                taskItemDto.Title,
                taskItemDto.Description,
                taskItemDto.ProjectId,
                taskItemDto.AssigneeId,
                Priority.Medium
            );

            await _taskItemRepository.AddAsync(taskItem);
            return _mapper.Map<TaskItemResponseDto>(taskItem);
        }

        public async Task<TaskItemResponseDto?> GetTaskItemByIdAsync(Guid id)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(id);
            return _mapper.Map<TaskItemResponseDto>(taskItem);
        }

        public async Task<IEnumerable<TaskItemResponseDto>> GetTaskItemsByProjectIdAsync(Guid projectId)
        {
            var taskItems = await _taskItemRepository.GetByProjectIdAsync(projectId);
            return _mapper.Map<IEnumerable<TaskItemResponseDto>>(taskItems);
        }

        public async Task<TaskItemResponseDto?> UpdateTaskItemAsync(Guid id, UpdateTaskItemDto taskItemDto)
        {
            var existingTaskItem = await _taskItemRepository.GetByIdAsync(id);
            if (existingTaskItem == null)
            {
                return null;
            }

            if (taskItemDto.Title != null)
            {
                existingTaskItem.UpdateTitle(taskItemDto.Title);
            }

            if (taskItemDto.Description != null)
            {
                existingTaskItem.UpdateDescription(taskItemDto.Description);
            }

            if (taskItemDto.Status != null)
            {
                existingTaskItem.UpdateStatus(taskItemDto.Status.Value);
            }


            if (taskItemDto.AssigneeId != null)
            {
                existingTaskItem.AssignToUser(taskItemDto.AssigneeId);
            }

            await _taskItemRepository.UpdateAsync(existingTaskItem);
            return _mapper.Map<TaskItemResponseDto>(existingTaskItem);
        }

        public async Task<bool> DeleteTaskItemAsync(Guid id)
        {
            var taskItem = await _taskItemRepository.GetByIdAsync(id);
            if (taskItem == null)
            {
                return false; 
            }
            await _taskItemRepository.DeleteAsync(taskItem);
            return true;
        }
    }
}