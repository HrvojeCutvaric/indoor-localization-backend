namespace IndoorLocalization.Models.DTOs
{
    public class AssetZoneHistoryResponse
    {
        public long Id { get; set; }
        public long? AssetId { get; set; }
        public long? ZoneId { get; set; }
        public DateTime EnterDateTime { get; set; }
        public DateTime? ExitDateTime { get; set; }
        public TimeSpan? RetentionTime { get; set; }
    }
}
