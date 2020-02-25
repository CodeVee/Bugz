using System;

namespace Entities.Models
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public string Message { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid? TaskId { get; set; }
        public Task Task { get; set; }
        public Guid? IssueId { get; set; }
        public Issue Issue { get; set; }
    }
}