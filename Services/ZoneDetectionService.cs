using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Services.Interfaces;
using System.Text.Json;

namespace IndoorLocalization.Services
{
    public class ZoneDetectionService : IZoneDetectionService
    {
        private record Point2D(double X, double Y);

        private readonly IZoneService _zoneService;
        private readonly IAssetZoneHistoryService _assetZoneHistoryService;

        public ZoneDetectionService(IZoneService zoneService, IAssetZoneHistoryService assetZoneHistoryService)
        {
            _zoneService = zoneService;
            _assetZoneHistoryService = assetZoneHistoryService;
        }

        public async Task ProcessAssetPositionAsync(
           long assetId,
           double x,
           double y,
           long floorMapId,
           DateTime timestamp)
        {
            var zones = await _zoneService.GetByFloorMapAsync(floorMapId);

            var activeZoneEntries = await _assetZoneHistoryService.GetActiveZonesAsync(assetId);

            foreach (var zone in zones)
            {
                if (string.IsNullOrWhiteSpace(zone.Points))
                    continue;

                var pointDtos = JsonSerializer.Deserialize<List<ZonePointDto>>(zone.Points);
                if (pointDtos == null || pointDtos.Count != 4)
                    continue;

                var points = pointDtos
                    .OrderBy(p => p.OrdinalNumber)
                    .Select(p => new Point2D(p.X, p.Y))
                    .ToList();

                bool isInside = IsInsideRectangle(x, y, points[0], points[1], points[2], points[3]);

                var activeEntry = activeZoneEntries.FirstOrDefault(e => e.ZoneId == zone.Id);

                if (isInside && activeEntry == null)
                {
                    await _assetZoneHistoryService.AddEnterAsync(assetId, zone.Id, timestamp);

                }

                if (!isInside && activeEntry != null)
                {
                    await _assetZoneHistoryService.CloseAsync(activeEntry, timestamp);
                }

            }

        }

        private bool IsInsideRectangle(
                double assetX,
                double assetY,
                Point2D p1,
                Point2D p2,
                Point2D p3,
                Point2D p4)
        {
            var minX = Math.Min(Math.Min(p1.X, p2.X), Math.Min(p3.X, p4.X));
            var maxX = Math.Max(Math.Max(p1.X, p2.X), Math.Max(p3.X, p4.X));

            var minY = Math.Min(Math.Min(p1.Y, p2.Y), Math.Min(p3.Y, p4.Y));
            var maxY = Math.Max(Math.Max(p1.Y, p2.Y), Math.Max(p3.Y, p4.Y));

            return assetX > minX &&
                   assetX < maxX &&
                   assetY > minY &&
                   assetY < maxY;
        }

    }

}
