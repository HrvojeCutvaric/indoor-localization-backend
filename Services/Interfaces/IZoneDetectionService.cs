namespace IndoorLocalization.Services.Interfaces
{
    public interface IZoneDetectionService
    {
        Task ProcessAssetPositionAsync(long assetId, double x, double y, long floorMapId, DateTime timestamp);
    }
}
