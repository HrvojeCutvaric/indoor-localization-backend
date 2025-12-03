using IndoorLocalization.Models.DTOs;

namespace IndoorLocalization.Services.Interfaces
{
    public interface IFloorMapService
    {
        Task<List<FloorMapResponseDto>> GetAllAsync();
        Task<FloorMapResponseDto?> GetByIdAsync(long id);
        Task<FloorMapResponseDto?> GetByNameAsync(string name);

        Task<FloorMapResponseDto> CreateAsync(FloorMapCreateRequestDto dto);
        Task<FloorMapResponseDto?> UpdateAsync(long id, FloorMapUpdateRequestDto dto);

        Task<bool> DeleteAsync(long id);

        Task<List<AssetResponseDto>> GetAssetsByFloorMapAsync(long mapId);
    }
}
