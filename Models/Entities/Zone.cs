using System;
using System.Collections.Generic;

namespace IndoorLocalization.Models.Entities;

public class Zone
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Points { get; set; } // JSON
    public long FloorMapId { get; set; }
    public long? UserId { get; set; }

    public virtual FloorMap FloorMap { get; set; } = null!;
    public virtual User? User { get; set; }
    public virtual ICollection<AssetZoneHistory> AssetZoneHistories { get; set; } = new List<AssetZoneHistory>();
}
