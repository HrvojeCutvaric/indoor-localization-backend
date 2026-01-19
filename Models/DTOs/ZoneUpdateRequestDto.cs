namespace IndoorLocalization.Models.DTOs
{
    public class ZoneUpdateRequestDto
    {
        public string Name { get; set; } = null!;
        public List<ZonePointDto> Points { get; set; } = new();
    }
}
