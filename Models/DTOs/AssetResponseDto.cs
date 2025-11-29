namespace IndoorLocalization.Models.DTOs
{
    public class AssetResponseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public double? X { get; set; }
        public double? Y { get; set; }
        public DateTime? LastSync { get; set; }
        public bool Active { get; set; }

    }
}
