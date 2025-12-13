using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using IndoorLocalization.Services.Interfaces;

namespace IndoorLocalization.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;

        public AssetService(IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }

        public async Task<IEnumerable<AssetResponseDto>> GetAllAsync()
        {
            var assets = await _assetRepository.GetAllAsync();

            return assets.Select(a => new AssetResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                X = a.X,
                Y = a.Y,
                LastSync = a.LastSync,
                FloorMapId = a.FloorMapId,
                Active = a.Active,
                Color = a.Color
            });
        }

        public async Task<AssetResponseDto?> GetByIdAsync(long id)
        {
            var a = await _assetRepository.GetByIdAsync(id);
            if (a == null) return null;

            return new AssetResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                X = a.X,
                Y = a.Y,
                LastSync = a.LastSync,
                FloorMapId = a.FloorMapId,
                Active = a.Active,
                Color = a.Color
            };
        }

        public async Task<AssetResponseDto?> GetByNameAsync(string name)
        {
            var a = await _assetRepository.GetByNameAsync(name);
            if (a == null) return null;

            return new AssetResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                X = a.X,
                Y = a.Y,
                LastSync = a.LastSync,
                FloorMapId = a.FloorMapId,
                Active = a.Active,
                Color = a.Color
            };
        }

        public async Task<IEnumerable<AssetResponseDto>> GetAssetsByFloorMapAsync(long floorMapId)
        {
            var list = await _assetRepository.GetAssetsByFloorMapIdAsync(floorMapId);

            return list.Select(a => new AssetResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                X = a.X,
                Y = a.Y,
                LastSync = a.LastSync,
                FloorMapId = a.FloorMapId,
                Active = a.Active,
                Color = a.Color
            });
        }

        public async Task<AssetResponseDto> CreateAsync(AssetCreateRequestDto dto)
        {
            if (await _assetRepository.ExistsByNameAsync(dto.Name))
                throw new ArgumentException("Asset name already exists.");

            var asset = new Asset
            {
                Name = dto.Name,
                X = dto.X,
                Y = dto.Y,
                FloorMapId = dto.FloorMapId,
                Active = dto.Active,
                Color = dto.Color ?? "#FF0000"
            };

            await _assetRepository.AddAsync(asset);

            return new AssetResponseDto
            {
                Id = asset.Id,
                Name = asset.Name,
                X = asset.X,
                Y = asset.Y,
                LastSync = asset.LastSync,
                FloorMapId = asset.FloorMapId,
                Active = asset.Active,
                Color = asset.Color
            };
        }

        public async Task<bool> UpdateNameAndColorAsync(long id, AssetUpdateNameColorRequestDto dto)
        {
            return await _assetRepository.UpdateNameAndColorAsync(id, dto.Name ?? "", dto.Color ?? "");
        }

        public async Task<bool> UpdateStatusAsync(long id, AssetUpdateStatusRequestDto dto)
        {
            return await _assetRepository.UpdateStatusAsync(id, dto.Active);
        }

        public async Task<bool> UpdateCoordinatesAsync(long id, AssetUpdateCoordinatesRequestDto dto)
        {
            return await _assetRepository.UpdateCoordinates(id, dto.X, dto.Y);
        }

        public async Task<bool> UpdateFloorMapAsync(long id, AssetUpdateFloorMapRequestDto dto)
        {
            return await _assetRepository.UpdateFloorMap(id, dto.FloorMapId);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            if (asset == null) return false;

            return await _assetRepository.DeleteAsync(asset);
        }

        public async Task<IEnumerable<AssetPositionHistoryResponseDto>> GetPositionHistoryAsync(long assetId)
        {
            var list = await _assetRepository.GetPositionHistoryAsync(assetId);

            return list.Select(h => new AssetPositionHistoryResponseDto
            {
                Id = h.Id,
                AssetId = h.AssetId,
                FloorMapId = h.FloorMapId,
                X = h.X,
                Y = h.Y,
                DateTime = h.DateTime
            });
        }

        public async Task<bool> AddPositionHistoryAsync(long assetId, double x, double y, long floorMapId, DateTime timestamp)
        {
            return await _assetRepository.AddPositionHistoryAsync(assetId, x, y, floorMapId, timestamp);
        }

        public async Task<IEnumerable<AssetZoneHistoryResponseDto>> GetZoneHistoryAsync(long assetId)
        {
            var list = await _assetRepository.GetZoneHistoryAsync(assetId);

            return list.Select(h => new AssetZoneHistoryResponseDto
            {
                Id = h.Id,
                AssetId = h.AssetId,
                ZoneId = h.ZoneId,
                EnterDateTime = h.EnterDateTime,
                ExitDateTime = h.ExitDateTime,
                RetentionTime = h.RetentionTime
            });
        }
    }
}
