using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using IndoorLocalization.Services.Interfaces;

namespace IndoorLocalization.Services
{
    public class AssetZoneHistoryService : IAssetZoneHistoryService
    {
        private readonly IAssetZoneHistoryRepository _repository;

        public AssetZoneHistoryService(IAssetZoneHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AssetZoneHistory>> GetActiveZonesAsync(long assetId)
        {
            return await _repository.GetActiveZonesAsync(assetId);
        }

        public async Task<IEnumerable<AssetZoneHistory>> GetHistoryByAssetAsync(long assetId)
        {
            return await _repository.GetByAssetIdAsync(assetId);
        }

        public async Task AddEnterAsync(long assetId, long zoneId, DateTime timestamp)
        {
            var history = new AssetZoneHistory
            {
                AssetId = assetId,
                ZoneId = zoneId,
                EnterDateTime = timestamp
            };

            await _repository.AddAsync(history);
        }

        public async Task CloseAsync(AssetZoneHistory history, DateTime exitTime)
        {
            history.ExitDateTime = exitTime;
            history.RetentionTime = exitTime - history.EnterDateTime;

            await _repository.UpdateAsync(history);
        }
    }
}
