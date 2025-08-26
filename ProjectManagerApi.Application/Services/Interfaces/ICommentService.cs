using ProjectManagerApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagerApi.Application.Services.Interfaces
{
    public interface ICommentService
    {
        /// <summary>
        /// Creates a new comment.
        /// </summary>
        /// <param name="commentDto">The DTO containing comment creation data.</param>
        /// <returns>A DTO representing the created comment.</returns>
        Task<CommentResponseDto> CreateCommentAsync(CreateCommentDto commentDto);

        /// <summary>
        /// Retrieves a comment by its unique ID.
        /// </summary>
        /// <param name="id">The comment's unique ID.</param>
        /// <returns>A DTO representing the comment, or null if not found.</returns>
        Task<CommentResponseDto?> GetCommentByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a collection of comments associated with a specific task item.
        /// </summary>
        /// <param name="taskItemId">The ID of the task item to retrieve comments for.</param>
        /// <returns>A collection of DTOs representing comments for the given task item.</returns>
        Task<IEnumerable<CommentResponseDto>> GetCommentsByTaskItemIdAsync(Guid taskItemId);

        /// <summary>
        /// Updates an existing comment's information.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="commentDto">The DTO containing updated comment data.</param>
        /// <returns>A DTO representing the updated comment, or null if the comment was not found.</returns>
        Task<CommentResponseDto?> UpdateCommentAsync(Guid id, UpdateCommentDto commentDto);

        /// <summary>
        /// Deletes a comment by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        /// <returns>True if the comment was successfully deleted, false otherwise.</returns>
        Task<bool> DeleteCommentAsync(Guid id);
    }
}