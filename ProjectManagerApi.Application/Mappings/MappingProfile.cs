using AutoMapper;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Core.Entities;

namespace ProjectManagerApi.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mappings
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UserResponseDto>();

            // Project Mappings
            CreateMap<CreateProjectDto, Project>();
            CreateMap<UpdateProjectDto, Project>();
            CreateMap<Project, ProjectResponseDto>();

            // TaskItem Mappings
            CreateMap<CreateTaskItemDto, TaskItem>();
            CreateMap<UpdateTaskItemDto, TaskItem>();
            CreateMap<TaskItem, TaskItemResponseDto>();

            // Comment Mappings
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<UpdateCommentDto, Comment>();
            CreateMap<Comment, CommentResponseDto>();
        }
    }
}