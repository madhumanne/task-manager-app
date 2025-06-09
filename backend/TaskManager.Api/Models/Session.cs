using System;

namespace TaskManager.Api.Models
{
    public class Session
    {
        public int Id { get; set; }
        public Guid SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime Expiry { get; set; }
        public bool IsExpired { get; set; }
        public User User { get; set; }
    }
}
