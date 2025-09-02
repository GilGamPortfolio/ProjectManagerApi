using AutoMapper;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Application.Interfaces; // Para ITaskItemRepository
using ProjectManagerApi.Application.Services.Interfaces; // Para ITaskItemService
using ProjectManagerApi.Core.Entities;
using ProjectManagerApi.Core.Enums;

namespace ProjectManagerApi.Application.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;
        private readonly IMapper _mapper;
        // Opcional: private readonly IProjectService _projectService; // Para validar projectId
        // Opcional: private readonly IUserService _userService;     // Para validar AssigneeId

        // Injeção de dependência do repositório e do mapper
        public TaskItemService(ITaskItemRepository taskItemRepository, IMapper mapper)
        {
            _taskItemRepository = taskItemRepository;
            _mapper = mapper;
        }

        public async Task<TaskItemResponseDto> CreateTaskItemAsync(CreateTaskItemDto taskItemDto)
        {
            // TODO: Adicionar validação de negócio:
            // - Verificar se ProjectId existe (opcional, mas recomendado para integridade referencial)
            // - Verificar se AssigneeId existe, se fornecido (opcional, mas recomendado)

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
                return null; // TaskItem não encontrado
            }

            // Atualiza as propriedades da entidade TaskItem usando seus métodos internos.
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
                // Garante que o status é um valor válido do enum antes de atualizar
                existingTaskItem.UpdateStatus(taskItemDto.Status.Value);
            }

            // TODO: Adicionar validação de negócio para AssigneeId:
            // - Verificar se AssigneeId existe, se fornecido e não for null.
            if (taskItemDto.AssigneeId != null) // Se um valor for fornecido (mesmo que seja null para desatribuir)
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
                return false; // TaskItem não encontrado
            }
            await _taskItemRepository.DeleteAsync(taskItem);
            return true;
        }
    }
}