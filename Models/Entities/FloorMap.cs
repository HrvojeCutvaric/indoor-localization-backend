using System;
using System.Collections.Generic;

namespace IndoorLocalization.Models.Entities;

public class FloorMap
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public int? ImageWidthPx { get; set; }
    public int? ImageHeightPx { get; set; }
    public double WidthInMeters { get; set; }
    public double HeightInMeters { get; set; }

    public virtual ICollection<Zone> Zones { get; set; } = new List<Zone>();
    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
    public virtual ICollection<AssetPositionHistory> AssetPositionHistories { get; set; } = new List<AssetPositionHistory>();
}
