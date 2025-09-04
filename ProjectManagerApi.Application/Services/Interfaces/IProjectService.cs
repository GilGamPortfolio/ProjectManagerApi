using ProjectManagerApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagerApi.Application.Services.Interfaces
{
    public interface IProjectService
    {
        /// <summary>
        /// Creates a new project.
        /// </summary>
        /// <param name="projectDto">The DTO containing project creation data.</param>
        /// <returns>A DTO representing the created project.</returns>
        Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto projectDto);

        /// <summary>
        /// Retrieves a project by its unique ID.
        /// </summary>
        /// <param name="id">The project's unique ID.</param>
        /// <returns>A DTO representing the project, or null if not found.</returns>
        Task<ProjectResponseDto?> GetProjectByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <returns>A collection of DTOs representing all projects.</returns>
        Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync();

        /// <summary>
        /// Updates an existing project's information.
        /// </summary>
        /// <param name="id">The ID of the project to update.</param>
        /// <param name="projectDto">The DTO containing updated project data.</param>
        /// <returns>A DTO representing the updated project, or null if the project was not found.</returns>
        Task<ProjectResponseDto?> UpdateProjectAsync(Guid id, UpdateProjectDto projectDto);

        /// <summary>
        /// Deletes a project by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the project to delete.</param>
        /// <returns>True if the project was successfully deleted, false otherwise.</returns>
        Task<bool> DeleteProjectAsync(Guid id);

        /// <summary>
        /// Retrieves an enumerable collection of projects associated with a specific owner asynchronously.
        /// </summary>
        /// <param name="ownerId">The unique identifier of the owner whose projects are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/> of <see cref="ProjectResponseDto"/> objects.</returns>
        Task<IEnumerable<ProjectResponseDto>> GetProjectsByOwnerIdAsync(Guid ownerId);
    }
}