namespace IndoorLocalization.Models.DTOs
{
    public class FloorMapCreateRequestDto
    {
        public string Name { get; set; } = null!;

        public IFormFile? ImageFile { get; set; }

        public int? ImageWidthPx { get; set; }
        public int? ImageHeightPx { get; set; }
        public double WidthM { get; set; }
        public double HeightM { get; set; }
    }
}
