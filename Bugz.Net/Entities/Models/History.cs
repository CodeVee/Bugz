using System;

namespace Entities.Models
{
    public class History
    {
        public Guid HistoryId { get; set; }
        public string Property { get; set; }
        public string Previous { get; set; }
        public string Current { get; set; }
        public DateTime Time = DateTime.Now;
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid? TaskId { get; set; }
        public Task Task { get; set; }
        public Guid? IssueId { get; set; }
        public Issue Issue { get; set; }
    }
}