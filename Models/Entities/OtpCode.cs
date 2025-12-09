using System;

namespace IndoorLocalization.Models.Entities
{
    public class OtpCode
    {
        public long Id { get; set; }
        public string CodeHash { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public int Attempts { get; set; } = 0;
        public bool IsUsed { get; set; } = false;

        public long UserId { get; set; }
        public virtual User? User { get; set; }
    }
}