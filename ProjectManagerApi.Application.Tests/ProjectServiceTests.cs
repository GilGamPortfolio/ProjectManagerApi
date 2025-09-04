using Xunit;
using Moq;
using AutoMapper;
using System;
using System.Threading.Tasks;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Application.Interfaces;
using ProjectManagerApi.Application.Services;
using ProjectManagerApi.Core.Entities;

namespace ProjectManagerApi.Application.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProjectService _projectService;

        public ProjectServiceTests()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockMapper = new Mock<IMapper>();
            _projectService = new ProjectService(_mockProjectRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateProjectAsync_ShouldCreateProjectAndReturnDto()
        {
            // Arrange
            var createProjectDto = new CreateProjectDto
            {
                Name = "My Test Project",
                Description = "A description for my test project",
                OwnerId = Guid.NewGuid()
            };

            _mockProjectRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Project>()))
                .Returns(Task.CompletedTask); 

            var expectedProjectResponseDto = new ProjectResponseDto
            {
                Id = Guid.NewGuid(), 
                Name = createProjectDto.Name,
                Description = createProjectDto.Description,
                OwnerId = createProjectDto.OwnerId
            };

            _mockMapper
                .Setup(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()))
                .Returns(expectedProjectResponseDto);

            // Act
            var result = await _projectService.CreateProjectAsync(createProjectDto);

            // Assert
            _mockProjectRepository.Verify(repo => repo.AddAsync(It.Is<Project>(p =>
                p.Name == createProjectDto.Name &&
                p.Description == createProjectDto.Description &&
                p.OwnerId == createProjectDto.OwnerId
            )), Times.Once);

            _mockMapper.Verify(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(expectedProjectResponseDto.Name, result.Name);
            Assert.Equal(expectedProjectResponseDto.Description, result.Description);
            Assert.Equal(expectedProjectResponseDto.OwnerId, result.OwnerId);
        }

        [Fact]
        public async Task CreateProjectAsync_WhenDescriptionIsNull_ShouldCreateProjectWithEmptyDescription()
        {
            // Arrange
            var createProjectDto = new CreateProjectDto
            {
                Name = "Project Without Description",
                Description = null,
                OwnerId = Guid.NewGuid()
            };

            _mockProjectRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Project>()))
                .Returns(Task.CompletedTask);

            var expectedProjectResponseDto = new ProjectResponseDto
            {
                Id = Guid.NewGuid(),
                Name = createProjectDto.Name,
                Description = string.Empty, 
                OwnerId = createProjectDto.OwnerId
            };
            _mockMapper
                .Setup(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()))
                .Returns(expectedProjectResponseDto);

            // Act
            var result = await _projectService.CreateProjectAsync(createProjectDto);

            // Assert
            _mockProjectRepository.Verify(repo => repo.AddAsync(It.Is<Project>(p =>
                p.Name == createProjectDto.Name &&
                p.Description == string.Empty && 
                p.OwnerId == createProjectDto.OwnerId
            )), Times.Once);

            _mockMapper.Verify(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(expectedProjectResponseDto.Name, result.Name);
            Assert.Equal(expectedProjectResponseDto.Description, result.Description); 
            Assert.Equal(expectedProjectResponseDto.OwnerId, result.OwnerId);
        }

        [Fact]
        public async Task CreateProjectAsync_WhenNameIsEmpty_ShouldThrowArgumentException()
        {
            // Arrange
            var createProjectDto = new CreateProjectDto
            {
                Name = "", 
                Description = "Some description",
                OwnerId = Guid.NewGuid()
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _projectService.CreateProjectAsync(createProjectDto));

            _mockProjectRepository.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Never);
            _mockMapper.Verify(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()), Times.Never);
        }

        [Fact]
        public async Task CreateProjectAsync_WhenNameIsNull_ShouldThrowArgumentException()
        {
            // Arrange
            var createProjectDto = new CreateProjectDto
            {
                Name = null, 
                Description = "Some description",
                OwnerId = Guid.NewGuid()
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _projectService.CreateProjectAsync(createProjectDto));

            _mockProjectRepository.Verify(repo => repo.AddAsync(It.IsAny<Project>()), Times.Never);
            _mockMapper.Verify(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()), Times.Never);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ShouldReturnProject_WhenProjectExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project("Existing Project", "Description", Guid.NewGuid());

            _mockProjectRepository
                .Setup(repo => repo.GetByIdAsync(projectId))
                .ReturnsAsync(project);

            var expectedProjectResponseDto = new ProjectResponseDto
            {
                Id = projectId,
                Name = project.Name,
                Description = project.Description,
                OwnerId = project.OwnerId
            };

            _mockMapper
                .Setup(m => m.Map<ProjectResponseDto>(project))
                .Returns(expectedProjectResponseDto);

            // Act
            var result = await _projectService.GetProjectByIdAsync(projectId);

            // Assert
            _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);
            _mockMapper.Verify(m => m.Map<ProjectResponseDto>(project), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(expectedProjectResponseDto.Id, result.Id);
            Assert.Equal(expectedProjectResponseDto.Name, result.Name);
            Assert.Equal(expectedProjectResponseDto.Description, result.Description);
            Assert.Equal(expectedProjectResponseDto.OwnerId, result.OwnerId);
        }

        [Fact]
        public async Task GetProjectByIdAsync_ShouldReturnNull_WhenProjectDoesNotExist()
        {
            // Arrange
            var nonExistentProjectId = Guid.NewGuid();

            _mockProjectRepository
                .Setup(repo => repo.GetByIdAsync(nonExistentProjectId))
                .ReturnsAsync((Project)null);

            // Act
            var result = await _projectService.GetProjectByIdAsync(nonExistentProjectId);

            // Assert
            _mockProjectRepository.Verify(repo => repo.GetByIdAsync(nonExistentProjectId), Times.Once);

            _mockMapper.Verify(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()), Times.Never);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetProjectsByOwnerIdAsync_ShouldReturnProjects_WhenOwnerHasProjects()
        {
            // Arrange
            var ownerId = Guid.NewGuid();
            var projects = new List<Project>
            {
                new Project("Project 1", "Desc 1", ownerId),
                new Project("Project 2", "Desc 2", ownerId)
            };

            _mockProjectRepository
                .Setup(repo => repo.GetProjectsByOwnerIdAsync(ownerId))
                .ReturnsAsync(projects);

            var expectedProjectResponseDtos = new List<ProjectResponseDto>
            {
                new ProjectResponseDto { Id = Guid.NewGuid(), Name = "Project 1", Description = "Desc 1", OwnerId = ownerId },
                new ProjectResponseDto { Id = Guid.NewGuid(), Name = "Project 2", Description = "Desc 2", OwnerId = ownerId }
            };

            _mockMapper
                .Setup(m => m.Map<IEnumerable<ProjectResponseDto>>(projects))
                .Returns(expectedProjectResponseDtos);

            // Act
            var result = await _projectService.GetProjectsByOwnerIdAsync(ownerId);

            // Assert
            _mockProjectRepository.Verify(repo => repo.GetProjectsByOwnerIdAsync(ownerId), Times.Once);

            _mockMapper.Verify(m => m.Map<IEnumerable<ProjectResponseDto>>(projects), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(expectedProjectResponseDtos.Count, result.Count());

            Assert.Contains(result, p => p.Name == "Project 1");
            Assert.Contains(result, p => p.Name == "Project 2");
        }

        [Fact]
        public async Task GetProjectsByOwnerIdAsync_ShouldReturnEmptyList_WhenOwnerHasNoProjects()
        {
            // Arrange
            var ownerId = Guid.NewGuid();

            _mockProjectRepository
                .Setup(repo => repo.GetProjectsByOwnerIdAsync(ownerId))
                .ReturnsAsync(new List<Project>()); 

            _mockMapper
                .Setup(m => m.Map<IEnumerable<ProjectResponseDto>>(It.IsAny<IEnumerable<Project>>()))
                .Returns(new List<ProjectResponseDto>()); 

            // Act
            var result = await _projectService.GetProjectsByOwnerIdAsync(ownerId);

            // Assert
            _mockProjectRepository.Verify(repo => repo.GetProjectsByOwnerIdAsync(ownerId), Times.Once);

            _mockMapper.Verify(m => m.Map<IEnumerable<ProjectResponseDto>>(It.IsAny<IEnumerable<Project>>()), Times.Once);

            Assert.NotNull(result);
            Assert.Empty(result); 
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldUpdateProjectAndReturnUpdatedDto_WhenProjectExists()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var originalOwnerId = Guid.NewGuid();
            var newOwnerId = Guid.NewGuid();

            var originalProject = new Project("Original Name", "Original Description", originalOwnerId);
            typeof(Project).GetProperty("Id")?.SetValue(originalProject, projectId);

            var updateDto = new UpdateProjectDto
            {
                Name = "Updated Name",
                Description = "Updated Description",
                OwnerId = newOwnerId 
            };

            _mockProjectRepository
                .Setup(repo => repo.GetByIdAsync(projectId))
                .ReturnsAsync(originalProject);
             
            _mockMapper
                .Setup(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()))
                .Returns((Project p) => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    OwnerId = p.OwnerId
                });

            // Act
            var result = await _projectService.UpdateProjectAsync(projectId, updateDto);

            // Assert
            _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);

            _mockProjectRepository.Verify(repo => repo.UpdateAsync(
                It.Is<Project>(p => p.Id == projectId && 
                                     p.Name == updateDto.Name && 
                                     p.Description == updateDto.Description && 
                                     p.OwnerId == updateDto.OwnerId.Value)),
                                     Times.Once);

            _mockMapper.Verify(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(projectId, result.Id);
            Assert.Equal(updateDto.Name, result.Name);
            Assert.Equal(updateDto.Description, result.Description);
            Assert.Equal(updateDto.OwnerId.Value, result.OwnerId);
        }

        [Fact]
        public async Task UpdateProjectAsync_ShouldReturnNull_WhenProjectDoesNotExist()
        {
            // Arrange
            var nonExistentProjectId = Guid.NewGuid();
            var updateDto = new UpdateProjectDto
            {
                Name = "Updated Name",
                Description = "Updated Description",
                OwnerId = Guid.NewGuid()
            };

            _mockProjectRepository
                .Setup(repo => repo.GetByIdAsync(nonExistentProjectId))
                .ReturnsAsync((Project?)null);

            // Act
            var result = await _projectService.UpdateProjectAsync(nonExistentProjectId, updateDto);

            // Assert
            _mockProjectRepository.Verify(repo => repo.GetByIdAsync(nonExistentProjectId), Times.Once);

            _mockProjectRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Project>()), Times.Never);

            _mockMapper.Verify(m => m.Map<ProjectResponseDto>(It.IsAny<Project>()), Times.Never);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnTrue_WhenProjectExistsAndIsDeleted()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var projectToDelete = new Project("Test Project", "Description to delete", Guid.NewGuid());
            typeof(Project).GetProperty("Id")?.SetValue(projectToDelete, projectId);

            _mockProjectRepository
                .Setup(repo => repo.GetByIdAsync(projectId))
                .ReturnsAsync(projectToDelete);

            _mockProjectRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<Project>()))
                .Returns(Task.CompletedTask); // Simula uma Task concluída para o método async void/Task

            // Act
            var result = await _projectService.DeleteProjectAsync(projectId);

            // Assert
            _mockProjectRepository.Verify(repo => repo.GetByIdAsync(projectId), Times.Once);

            _mockProjectRepository.Verify(repo => repo.DeleteAsync(projectToDelete), Times.Once);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteProjectAsync_ShouldReturnFalse_WhenProjectDoesNotExist()
        {
            // Arrange
            var nonExistentProjectId = Guid.NewGuid();

            _mockProjectRepository
                .Setup(repo => repo.GetByIdAsync(nonExistentProjectId))
                .ReturnsAsync((Project?)null); 

            // Act
            var result = await _projectService.DeleteProjectAsync(nonExistentProjectId);

            // Assert
            _mockProjectRepository.Verify(repo => repo.GetByIdAsync(nonExistentProjectId), Times.Once);

            _mockProjectRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Project>()), Times.Never);

            Assert.False(result);
        }
    }
}