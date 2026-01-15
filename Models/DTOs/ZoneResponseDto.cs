namespace IndoorLocalization.Models.DTOs
{
    public class ZoneResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Points { get; set; } // JSON
        public long FloorMapId { get; set; }
        public long? UserId { get; set; }
    }
}
