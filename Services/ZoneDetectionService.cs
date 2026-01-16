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

                var polygon = pointDtos
                    .OrderBy(p => p.OrdinalNumber)
                    .Select(p => new Point2D(p.X, p.Y))
                    .ToList();

                bool isInside = IsPointInsidePolygon(x, y, polygon);

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

        private bool IsPointInsidePolygon(double x, double y, IReadOnlyList<Point2D> polygon)
        {
            bool inside = false;

            for (int i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
            {
                var pi = polygon[i];
                var pj = polygon[j];

                bool intersect =
                    ((pi.Y > y) != (pj.Y > y)) &&
                    (x < (pj.X - pi.X) * (y - pi.Y) / (pj.Y - pi.Y) + pi.X);

                if (intersect)
                    inside = !inside;
            }

            return inside;
        }

    }

}
