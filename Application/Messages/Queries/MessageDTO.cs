using System;
using Domain.Entities;
using Domain.Enums;

namespace Application.Messages.Queries
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string SenderImageUrl { get; set; }
        public MessageStatus Status { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public long? FileId { get; set; }
        public string FileName { get; set; }
        public bool SentByMe { get; set; }
    }
}