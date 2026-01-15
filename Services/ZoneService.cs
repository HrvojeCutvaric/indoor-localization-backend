using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using IndoorLocalization.Services.Interfaces;
using System.Text.Json;

namespace IndoorLocalization.Services
{
    public class ZoneService : IZoneService
    {
        private readonly IZoneRepository _zoneRepository;

        public ZoneService(IZoneRepository zoneRepository)
        {
            _zoneRepository = zoneRepository;
        }

        public async Task<IEnumerable<ZoneResponseDto>> GetAllAsync()
        {
            var zones = await _zoneRepository.GetAllAsync();

            return zones.Select(z => new ZoneResponseDto
            {
                Id = z.Id,
                Name = z.Name,
                Points = z.Points,
                FloorMapId = z.FloorMapId
            });
        }

        public async Task<IEnumerable<ZoneResponseDto>> GetByFloorMapAsync(long floorMapId)
        {
            var zones = await _zoneRepository.GetByFloorMapAsync(floorMapId);

            return zones.Select(z => new ZoneResponseDto
            {
                Id = z.Id,
                Name = z.Name,
                Points = z.Points,
                FloorMapId = z.FloorMapId
            });
        }

        public async Task<ZoneResponseDto?> GetByIdAsync(long id)
        {
            var zone = await _zoneRepository.GetByIdAsync(id);
            if (zone == null) return null;

            return new ZoneResponseDto
            {
                Id = zone.Id,
                Name = zone.Name,
                Points = zone.Points,
                FloorMapId = zone.FloorMapId
            };
        }

        public async Task<ZoneResponseDto> CreateAsync(ZoneCreateRequestDto dto)
        {
            var zone = new Zone
            {
                Name = dto.Name,
                Points = JsonSerializer.Serialize(dto.Points),
                FloorMapId = dto.FloorMapId
            };

            await _zoneRepository.AddAsync(zone);

            return new ZoneResponseDto
            {
                Id = zone.Id,
                Name = zone.Name,
                Points = zone.Points,
                FloorMapId = zone.FloorMapId
            };
        }

        public async Task<bool> UpdateAsync(long id, ZoneUpdateRequestDto dto)
        {
            var zone = await _zoneRepository.GetByIdAsync(id);
            if (zone == null) return false;

            zone.Name = dto.Name;
            zone.Points = JsonSerializer.Serialize(dto.Points);

            return await _zoneRepository.UpdateAsync(zone);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var zone = await _zoneRepository.GetByIdAsync(id);
            if (zone == null) return false;

            return await _zoneRepository.DeleteAsync(zone);
        }
    }
}
