namespace IndoorLocalization.Models.DTOs
{
    public class FloorMapUpdateRequestDto
    {
        public string? Name { get; set; }
        public IFormFile? ImageFile { get; set; }

        public int? ImageWidthPx { get; set; }
        public int? ImageHeightPx { get; set; }
        public double? WidthM { get; set; }
        public double? HeightM { get; set; }
    }
}
