namespace IndoorLocalization.Models.Entities
{
    public class User
    {
        public long Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ICollection<Zone>? Zones { get; set; }

    }
}
