using IndoorLocalization.Models.DTOs;

namespace IndoorLocalization.Services.Interfaces
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetResponseDto>> GetAllAsync();
        Task<AssetResponseDto?> GetByIdAsync(long id);
        Task<AssetResponseDto?> GetByNameAsync(string name);

        Task<IEnumerable<AssetResponseDto>> GetAssetsByFloorMapAsync(long floorMapId);

        Task<AssetResponseDto> CreateAsync(AssetCreateRequestDto dto);

        Task<bool> UpdateNameAndColorAsync(long id, AssetUpdateNameColorRequestDto dto);
        Task<bool> UpdateStatusAsync(long id, AssetUpdateStatusRequestDto dto);
        Task<bool> UpdateCoordinatesAsync(long id, AssetUpdateCoordinatesRequestDto dto);
        Task<bool> UpdateFloorMapAsync(long id, AssetUpdateFloorMapRequestDto dto);

        Task<bool> DeleteAsync(long id);

        Task<IEnumerable<AssetPositionHistoryResponseDto>> GetPositionHistoryAsync(long assetId);
        Task<IEnumerable<AssetZoneHistoryResponseDto>> GetZoneHistoryAsync(long assetId);
    }
}
