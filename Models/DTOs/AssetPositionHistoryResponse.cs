namespace IndoorLocalization.Models.DTOs
{
    public class AssetPositionHistoryResponse
    {
        public long Id { get; set; }
        public long? AssetId { get; set; }
        public long? FloorMapId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
