using AutoMapper;
using ProjectManagerApi.Application.DTOs;
using ProjectManagerApi.Application.Interfaces;
using ProjectManagerApi.Application.Services.Interfaces;
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

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto commentDto)
        {
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
                return null;
            }

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
                return false; 
            }
            await _commentRepository.DeleteAsync(comment);
            return true;
        }
    }
}