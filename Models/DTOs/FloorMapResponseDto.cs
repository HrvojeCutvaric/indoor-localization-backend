namespace IndoorLocalization.Models.DTOs
{
    public class FloorMapResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public int? ImageWidthPx { get; set; }
        public int? ImageHeightPx { get; set; }
        public double WidthInMeters { get; set; }
        public double HeightInMeters { get; set; }
    }
}
