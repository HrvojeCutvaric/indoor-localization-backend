using System;
using System.Collections.Generic;

namespace IndoorLocalization.Models.Entities;

public class Asset
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public double? X { get; set; }
    public double? Y { get; set; }
    public DateTime? LastSync { get; set; }
    public long? FloorMapId { get; set; }
    public bool Active { get; set; } = true;

    public virtual FloorMap? FloorMap { get; set; }
    public virtual ICollection<AssetPositionHistory> AssetPositionHistories { get; set; } = new List<AssetPositionHistory>();
    public virtual ICollection<AssetZoneHistory> AssetZoneHistories { get; set; } = new List<AssetZoneHistory>();
}
