namespace IndoorLocalization.Models.DTOs
{
    public class FloorMapResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
    }
}
