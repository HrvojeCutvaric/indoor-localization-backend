namespace IndoorLocalization.Entities
{
    public class Asset
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime LastSync { get; set; }
        public long FloorMapId { get; set; }
        public bool Active { get; set; }

        public FloorMap? FloorMap { get; set; }
        public ICollection<AssetPositionHistory>? AssetPositionHistories { get; set; }
        public ICollection<AssetZoneHistory>? AssetZoneHistories { get; set; }
    }
}
