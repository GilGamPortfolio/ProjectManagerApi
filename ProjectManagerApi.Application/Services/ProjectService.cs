using AutoMapper;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Application.Interfaces;
using ProjectManagerApi.Application.Services.Interfaces;
using ProjectManagerApi.Core.Entities;

namespace ProjectManagerApi.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto projectDto)
        {

            var project = new Project(projectDto.Name, projectDto.Description, projectDto.OwnerId);

            await _projectRepository.AddAsync(project);
            return _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);

            if (project == null) return null;

            return _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetProjectsByOwnerIdAsync(Guid ownerId)
        {
            var projects = await _projectRepository.GetProjectsByOwnerIdAsync(ownerId);
            return _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        }

        public async Task<ProjectResponseDto?> UpdateProjectAsync(Guid id, UpdateProjectDto projectDto)
        {
            var existingProject = await _projectRepository.GetByIdAsync(id);
            if (existingProject == null)
            {
                return null; 
            }

            if (projectDto.Name != null)
            {
                existingProject.UpdateName(projectDto.Name);
            }

            if (projectDto.Description != null)
            {
                existingProject.UpdateDescription(projectDto.Description);
            }

            if (projectDto.OwnerId != Guid.Empty && projectDto.OwnerId != null) 
            {
                existingProject.UpdateOwner(projectDto.OwnerId.Value); 
            }

            await _projectRepository.UpdateAsync(existingProject);
            return _mapper.Map<ProjectResponseDto>(existingProject);
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                return false; 
            }
            await _projectRepository.DeleteAsync(project);
            return true;
        }
    }
}