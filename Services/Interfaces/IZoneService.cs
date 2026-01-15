using IndoorLocalization.Models.DTOs;

namespace IndoorLocalization.Services.Interfaces
{
    public interface IZoneService
    {
        Task<IEnumerable<ZoneResponseDto>> GetAllAsync();
        Task<IEnumerable<ZoneResponseDto>> GetByFloorMapAsync(long floorMapId);
        Task<ZoneResponseDto?> GetByIdAsync(long id);
        Task<ZoneResponseDto> CreateAsync(ZoneCreateRequestDto dto);
        Task<bool> UpdateAsync(long id, ZoneUpdateRequestDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
