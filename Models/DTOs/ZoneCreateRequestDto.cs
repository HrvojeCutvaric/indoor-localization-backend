namespace IndoorLocalization.Models.DTOs
{
    public class ZoneCreateRequestDto
    {
        public string Name { get; set; } = null!;
        public List<ZonePointDto> Points { get; set; } = new();
        public long FloorMapId { get; set; }
    }
}
