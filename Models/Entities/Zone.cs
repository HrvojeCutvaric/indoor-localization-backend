namespace IndoorLocalization.Models.Entities
{
    public class Zone
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Points { get; set; } // JSON: lista točaka (x, y, ordinalNumber)
        public long UserId { get; set; }

        public User? User { get; set; }
        public ICollection<AssetZoneHistory>? AssetZoneHistories { get; set; }
    }
}
