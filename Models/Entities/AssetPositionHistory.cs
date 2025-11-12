namespace IndoorLocalization.Models.Entities
{
    public class AssetPositionHistory
    {
        public long Id { get; set; }
        public long AssetId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime DateTime { get; set; }
        public long FloorMapId { get; set; }

        public Asset? Asset { get; set; }
        public FloorMap? FloorMap { get; set; }
    }
}
