namespace IndoorLocalization.Models.DTOs
{
    public class FloorMapUpdateRequestDto
    {
        public string? Name { get; set; }
        public IFormFile? ImageFile { get; set; }

        public int? ImageWidthPx { get; set; }
        public int? ImageHeightPx { get; set; }
        public double? WidthInMeters { get; set; }
        public double? HeightInMeters { get; set; }
    }
}
