namespace IndoorLocalization.Models.DTOs
{
    public class AssetCreateRequestDto
    {
        public string Name { get; set; } = null!;
        public double? X { get; set; }
        public double? Y { get; set; }
        public long? FloorMapId { get; set; }
        public bool Active { get; set; } = true;
        public string? Color { get; set; }
    }
}
