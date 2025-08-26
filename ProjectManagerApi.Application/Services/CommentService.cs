using AutoMapper;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Application.Interfaces; // Para ICommentRepository
using ProjectManagerApi.Application.Services.Interfaces; // Para ICommentService
using ProjectManagerApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagerApi.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        // Opcional: private readonly ITaskItemService _taskItemService; // Para validar TaskItemId
        // Opcional: private readonly IUserService _userService;         // Para validar UserId

        // Injeção de dependência do repositório e do mapper
        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto commentDto)
        {
            // TODO: Adicionar validação de negócio:
            // - Verificar se TaskItemId existe (opcional, mas recomendado para integridade referencial)
            // - Verificar se UserId existe (opcional, mas recomendado)

            var comment = new Comment(
                commentDto.Content,
                commentDto.TaskItemId,
                commentDto.UserId
            );

            await _commentRepository.AddAsync(comment);
            return _mapper.Map<CommentResponseDto>(comment);
        }

        public async Task<CommentResponseDto?> GetCommentByIdAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            return _mapper.Map<CommentResponseDto>(comment);
        }

        public async Task<IEnumerable<CommentResponseDto>> GetCommentsByTaskItemIdAsync(Guid taskItemId)
        {
            var comments = await _commentRepository.GetByTaskItemIdAsync(taskItemId);
            return _mapper.Map<IEnumerable<CommentResponseDto>>(comments);
        }

        public async Task<CommentResponseDto?> UpdateCommentAsync(Guid id, UpdateCommentDto commentDto)
        {
            var existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
            {
                return null; // Comentário não encontrado
            }

            // Atualiza o conteúdo do comentário usando o método interno da entidade.
            if (commentDto.Content != null)
            {
                existingComment.UpdateContent(commentDto.Content);
            }

            await _commentRepository.UpdateAsync(existingComment);
            return _mapper.Map<CommentResponseDto>(existingComment);
        }

        public async Task<bool> DeleteCommentAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return false; // Comentário não encontrado
            }
            await _commentRepository.DeleteAsync(comment);
            return true;
        }
    }
}