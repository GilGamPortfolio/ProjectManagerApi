using ProjectManagerApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagerApi.Application.Services.Interfaces
{
    public interface ITaskItemService
    {
        /// <summary>
        /// Creates a new task item.
        /// </summary>
        /// <param name="taskItemDto">The DTO containing task item creation data.</param>
        /// <returns>A DTO representing the created task item.</returns>
        Task<TaskItemResponseDto> CreateTaskItemAsync(CreateTaskItemDto taskItemDto);

        /// <summary>
        /// Retrieves a task item by its unique ID.
        /// </summary>
        /// <param name="id">The task item's unique ID.</param>
        /// <returns>A DTO representing the task item, or null if not found.</returns>
        Task<TaskItemResponseDto?> GetTaskItemByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a collection of task items associated with a specific project.
        /// </summary>
        /// <param name="projectId">The ID of the project to retrieve task items for.</param>
        /// <returns>A collection of DTOs representing task items for the given project.</returns>
        Task<IEnumerable<TaskItemResponseDto>> GetTaskItemsByProjectIdAsync(Guid projectId);

        /// <summary>
        /// Updates an existing task item's information.
        /// </summary>
        /// <param name="id">The ID of the task item to update.</param>
        /// <param name="taskItemDto">The DTO containing updated task item data.</param>
        /// <returns>A DTO representing the updated task item, or null if the task item was not found.</returns>
        Task<TaskItemResponseDto?> UpdateTaskItemAsync(Guid id, UpdateTaskItemDto taskItemDto);

        /// <summary>
        /// Deletes a task item by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the task item to delete.</param>
        /// <returns>True if the task item was successfully deleted, false otherwise.</returns>
        Task<bool> DeleteTaskItemAsync(Guid id);
    }
}