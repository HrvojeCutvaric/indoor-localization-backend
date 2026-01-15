namespace IndoorLocalization.Models.DTOs
{
    public class ZoneCreateRequestDto
    {
        public string Name { get; set; } = null!;
        public string Points { get; set; } = null!; // JSON
        public long FloorMapId { get; set; }
    }
}
