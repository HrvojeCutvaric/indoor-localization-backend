using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using IndoorLocalization.Services.Interfaces;

namespace IndoorLocalization.Services
{
    public class FloorMapService : IFloorMapService
    {
        private readonly IFloorMapRepository _floorMapRepository;

        public FloorMapService(IFloorMapRepository floorMapRepository)
        {
            _floorMapRepository = floorMapRepository;
        }

        public async Task<List<FloorMapResponseDto>> GetAllAsync()
        {
            var floorMaps = await _floorMapRepository.GetAllAsync();
            return floorMaps.Select(fm => new FloorMapResponseDto
            {
                Id = fm.Id,
                Name = fm.Name,
                Image = fm.ImageUrl
            }).ToList();
        }

        public async Task<FloorMapResponseDto?> GetByIdAsync(long id)
        {
            var floorMap = await _floorMapRepository.GetByIdAsync(id);
            if (floorMap == null) return null;

            return new FloorMapResponseDto
            {
                Id = floorMap.Id,
                Name = floorMap.Name,
                Image = floorMap.ImageUrl
            };
        }

        public async Task<FloorMapResponseDto?> GetByNameAsync(string name)
        {
            var floorMap = await _floorMapRepository.GetByNameAsync(name);
            if (floorMap == null) return null;

            return new FloorMapResponseDto
            {
                Id = floorMap.Id,
                Name = floorMap.Name,
                Image = floorMap.ImageUrl
            };
        }

        public async Task<FloorMapResponseDto> CreateAsync(FloorMapCreateRequestDto dto)
        {
            if (await _floorMapRepository.ExistsByNameAsync(dto.Name))
                throw new InvalidOperationException("Map name already exists.");

            var map = new FloorMap
            {
                Name = dto.Name,
                ImageUrl = dto.Image
            };

            await _floorMapRepository.AddAsync(map);

            return new FloorMapResponseDto
            {
                Id = map.Id,
                Name = map.Name,
                Image = map.ImageUrl
            };
        }

        public async Task<FloorMapResponseDto?> UpdateAsync(long id, FloorMapUpdateRequestDto dto)
        {
            var existingMap = await _floorMapRepository.GetByIdAsync(id);
            if (existingMap == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                if (dto.Name != existingMap.Name &&
                    await _floorMapRepository.ExistsByNameAsync(dto.Name))
                {
                    throw new InvalidOperationException("Map name already exists.");
                }

                existingMap.Name = dto.Name;
            }

            if (!string.IsNullOrWhiteSpace(dto.Image))
            {
                existingMap.ImageUrl = dto.Image;
            }

            await _floorMapRepository.UpdateAsync(existingMap);

            return new FloorMapResponseDto
            {
                Id = existingMap.Id,
                Name = existingMap.Name,
                Image = existingMap.ImageUrl
            };
        }

        public async Task<List<AssetResponseDto>> GetAssetsByFloorMapAsync(long mapId)
        {
            var assets = await _floorMapRepository.GetAssetsByFloorMapIdAsync(mapId);
            return assets.Select(a => new AssetResponseDto
            {
                Id = a.Id,
                Name = a.Name,
                X = a.X,
                Y = a.Y,
                LastSync = a.LastSync,
                Active = a.Active

            }).ToList();
        }
    }
}
