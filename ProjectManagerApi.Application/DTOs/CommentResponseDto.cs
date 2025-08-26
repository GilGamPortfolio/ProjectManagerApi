using System;

namespace ProjectManagerApi.Application.DTOs
{
    public class CommentResponseDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid TaskItemId { get; set; }
        public Guid UserId { get; set; }
    }
}