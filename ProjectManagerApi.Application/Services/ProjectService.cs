using AutoMapper;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Application.Interfaces; // Para IProjectRepository
using ProjectManagerApi.Application.Services.Interfaces; // Para IProjectService
using ProjectManagerApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagerApi.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        // Injeção de dependência do repositório e do mapper
        public ProjectService(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto projectDto)
        {
            // Mapeia o DTO para a entidade, utilizando o construtor da entidade Project.
            // Certifique-se de que o AutoMapper está configurado para CreateMap<CreateProjectDto, Project>()
            // e que o construtor da entidade Project pode receber os parâmetros mapeados.
            var project = new Project(projectDto.Name, projectDto.Description, projectDto.OwnerId);

            await _projectRepository.AddAsync(project);
            return _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<ProjectResponseDto?> GetProjectByIdAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            return _mapper.Map<ProjectResponseDto>(project);
        }

        public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync()
        {
            // Este método depende de IProjectRepository.cs ter um método GetAllAsync()
            var projects = await _projectRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        }

        public async Task<ProjectResponseDto?> UpdateProjectAsync(Guid id, UpdateProjectDto projectDto)
        {
            var existingProject = await _projectRepository.GetByIdAsync(id);
            if (existingProject == null)
            {
                return null; // Projeto não encontrado
            }

            // Atualiza as propriedades da entidade Project usando seus métodos internos.
            if (projectDto.Name != null)
            {
                existingProject.UpdateName(projectDto.Name);
            }

            if (projectDto.Description != null)
            {
                existingProject.UpdateDescription(projectDto.Description);
            }

            // TODO: Validar se OwnerId existe como um usuário válido, se necessário.
            // Isso pode envolver injetar um IUserRepository ou IUserService aqui.
            if (projectDto.OwnerId != Guid.Empty && projectDto.OwnerId != null) // Verifica se o OwnerId foi fornecido no DTO
            {
                existingProject.UpdateOwner(projectDto.OwnerId.Value); // Usa .Value pois OwnerId é Guid? no DTO
            }

            await _projectRepository.UpdateAsync(existingProject);
            return _mapper.Map<ProjectResponseDto>(existingProject);
        }

        public async Task<bool> DeleteProjectAsync(Guid id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null)
            {
                return false; // Projeto não encontrado
            }
            await _projectRepository.DeleteAsync(project);
            return true;
        }
    }
}