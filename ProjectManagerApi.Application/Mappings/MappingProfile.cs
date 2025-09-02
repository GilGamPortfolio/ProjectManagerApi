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
            // Mapeamentos para User
            CreateMap<CreateUserDto, User>()
                .ConstructUsing(src => new User(src.Name, src.Email))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Para UpdateUserDto -> User
            // Propriedades do IdentityUser (como Email, UserName, PasswordHash)
            // DEVEM ser atualizadas via UserManager, NÃO diretamente pelo AutoMapper.
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null));
            // REMOVIDO: .ForAllOtherMembers(opt => opt.Ignore());
            // O AutoMapper, por padrão, ignorará as propriedades do DTO que não possuem
            // um mapeamento explícito (ForMember) ou que não correspondem na entidade.
            // Para Update, apenas as propriedades com Condition(src => src.Prop != null)
            // serão consideradas para atualização, protegendo as outras.

            CreateMap<User, UserResponseDto>();

            // Mapeamentos para Project
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
            // REMOVIDO: .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Project, ProjectResponseDto>();

            // Mapeamentos para TaskItem
            CreateMap<CreateTaskItemDto, TaskItem>()
                .ConstructUsing(src => new TaskItem(
                    src.Title,
                    src.Description != null ? src.Description : string.Empty,
                    src.ProjectId,
                    src.AssigneeId,
                    ProjectManagerApi.Core.Enums.Priority.Medium // Valor padrão assumido
                ))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<UpdateTaskItemDto, TaskItem>()
                .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
                .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
                .ForMember(dest => dest.Status, opt => opt.Condition(src => src.Status != null))
                .ForMember(dest => dest.AssigneeId, opt => opt.Condition(src => src.AssigneeId != null));
            // REMOVIDO: .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<TaskItem, TaskItemResponseDto>();

            // Mapeamentos para Comment
            CreateMap<CreateCommentDto, Comment>()
                // Lembre-se: CreateCommentDto precisa ter TaskItemId e UserId
                .ConstructUsing(src => new Comment(src.Content, src.TaskItemId, src.UserId))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<UpdateCommentDto, Comment>()
                .ForMember(dest => dest.Content, opt => opt.Condition(src => src.Content != null));
            // REMOVIDO: .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Comment, CommentResponseDto>();
        }
    }
}