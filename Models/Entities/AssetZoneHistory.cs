using System;
using System.Collections.Generic;

namespace IndoorLocalization.Models.Entities;

public partial class AssetZoneHistory
{
    public long Id { get; set; }
    public long? AssetId { get; set; }
    public long? ZoneId { get; set; }
    public DateTime EnterDateTime { get; set; }
    public DateTime? ExitDateTime { get; set; }
    public TimeSpan? RetentionTime { get; set; }

    public virtual Asset? Asset { get; set; }
    public virtual Zone? Zone { get; set; }
}
