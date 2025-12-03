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
    public double WidthM { get; set; }
    public double HeightM { get; set; }

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();
    public virtual ICollection<AssetPositionHistory> AssetPositionHistories { get; set; } = new List<AssetPositionHistory>();
}
