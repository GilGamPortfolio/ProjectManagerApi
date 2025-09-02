using AutoMapper;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Core.Entities;
using ProjectManagerApi.Core.Enums;
using System;

namespace ProjectManagerApi.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserDto, User>()
                .ConstructUsing(src => new User(src.Name, src.Email))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null));

            CreateMap<User, UserResponseDto>();

            CreateMap<CreateProjectDto, Project>()
                .ConstructUsing(src => new Project(
                    src.Name,
                    src.Description != null ? src.Description : string.Empty,
                    src.OwnerId
                ))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateProjectDto, Project>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
                .ForMember(dest => dest.OwnerId, opt => opt.Condition(src => src.OwnerId != null));

            CreateMap<Project, ProjectResponseDto>();

            CreateMap<CreateTaskItemDto, TaskItem>()
                .ConstructUsing(src => new TaskItem(
                    src.Title,
                    src.Description != null ? src.Description : string.Empty,
                    src.ProjectId,
                    src.AssigneeId,
                    ProjectManagerApi.Core.Enums.Priority.Medium
                ))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<UpdateTaskItemDto, TaskItem>()
                .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status != null))
                .ForMember(dest => dest.AssigneeId, opt => opt.Condition(src => src.AssigneeId != null));

            CreateMap<TaskItem, TaskItemResponseDto>();

            CreateMap<CreateCommentDto, Comment>()
                .ConstructUsing(src => new Comment(src.Content, src.TaskItemId, src.UserId))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateCommentDto, Comment>()
                .ForMember(dest => dest.Content, opt => opt.Condition(src => src.Content != null));

            CreateMap<Comment, CommentResponseDto>();
        }
    }
}