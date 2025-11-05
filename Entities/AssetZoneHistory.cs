namespace IndoorLocalization.Entities
{
    public class AssetZoneHistory
    {
        public long Id { get; set; }
        public long AssetId { get; set; }
        public long ZoneId { get; set; }
        public DateTime EnterDateTime { get; set; }
        public DateTime? ExitDateTime { get; set; }
        public TimeSpan? RetentionTime { get; set; }

        public Asset? Asset { get; set; }
        public Zone? Zone { get; set; }
    }
}
