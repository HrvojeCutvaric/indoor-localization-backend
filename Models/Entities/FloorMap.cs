namespace IndoorLocalization.Models.Entities
{
    public class FloorMap
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public ICollection<Asset>? Assets { get; set; }
        public ICollection<AssetPositionHistory>? AssetPositionHistories { get; set; }
    }
}
