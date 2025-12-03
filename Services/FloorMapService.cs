using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Repositories.Interfaces;
using IndoorLocalization.Services.Interfaces;

namespace IndoorLocalization.Services
{
    public class FloorMapService : IFloorMapService
    {
        private readonly IFloorMapRepository _floorMapRepository;
        private readonly IImageService _imageService;

        public FloorMapService(IFloorMapRepository floorMapRepository, IImageService imageService)
        {
            _floorMapRepository = floorMapRepository;
            _imageService = imageService;
        }

        public async Task<List<FloorMapResponseDto>> GetAllAsync()
        {
            var floorMaps = await _floorMapRepository.GetAllAsync();

            return floorMaps.Select(fm => new FloorMapResponseDto
            {
                Id = fm.Id,
                Name = fm.Name,
                ImageUrl = fm.ImageUrl,
                ImageWidthPx = fm.ImageWidthPx,
                ImageHeightPx = fm.ImageHeightPx,
                WidthM = fm.WidthM,
                HeightM = fm.HeightM
            }).ToList();
        }

        public async Task<FloorMapResponseDto?> GetByIdAsync(long id)
        {
            var floorMap = await _floorMapRepository.GetByIdAsync(id);
            if (floorMap == null)
                return null;

            return new FloorMapResponseDto
            {
                Id = floorMap.Id,
                Name = floorMap.Name,
                ImageUrl = floorMap.ImageUrl,
                ImageWidthPx = floorMap.ImageWidthPx,
                ImageHeightPx = floorMap.ImageHeightPx,
                WidthM = floorMap.WidthM,
                HeightM = floorMap.HeightM
            };
        }

        public async Task<FloorMapResponseDto?> GetByNameAsync(string name)
        {
            var floorMap = await _floorMapRepository.GetByNameAsync(name);
            if (floorMap == null)
                return null;

            return new FloorMapResponseDto
            {
                Id = floorMap.Id,
                Name = floorMap.Name,
                ImageUrl = floorMap.ImageUrl,
                ImageWidthPx = floorMap.ImageWidthPx,
                ImageHeightPx = floorMap.ImageHeightPx,
                WidthM = floorMap.WidthM,
                HeightM = floorMap.HeightM
            };
        }

        public async Task<FloorMapResponseDto> CreateAsync(FloorMapCreateRequestDto dto)
        {
            if (await _floorMapRepository.ExistsByNameAsync(dto.Name))
                throw new InvalidOperationException("Map name already exists.");

            string? imageUrl = null;

            if (dto.ImageFile != null)
                imageUrl = await _imageService.UploadImageAsync(dto.ImageFile);

            var map = new FloorMap
            {
                Name = dto.Name,
                ImageUrl = imageUrl,
                ImageWidthPx = dto.ImageWidthPx,
                ImageHeightPx = dto.ImageHeightPx,
                WidthM = dto.WidthM,
                HeightM = dto.HeightM
            };

            await _floorMapRepository.AddAsync(map);

            return new FloorMapResponseDto
            {
                Id = map.Id,
                Name = map.Name,
                ImageUrl = map.ImageUrl,
                ImageWidthPx = map.ImageWidthPx,
                ImageHeightPx = map.ImageHeightPx,
                WidthM = map.WidthM,
                HeightM = map.HeightM
            };
        }

        public async Task<FloorMapResponseDto?> UpdateAsync(long id, FloorMapUpdateRequestDto dto)
        {
            var existingMap = await _floorMapRepository.GetByIdAsync(id);
            if (existingMap == null)
                return null;

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                if (dto.Name != existingMap.Name &&
                    await _floorMapRepository.ExistsByNameAsync(dto.Name))
                {
                    throw new InvalidOperationException("Map name already exists.");
                }

                existingMap.Name = dto.Name;
            }

            if (dto.ImageFile != null)
            {
                var oldImageUrl = existingMap.ImageUrl;

                if (!string.IsNullOrEmpty(oldImageUrl))
                    await _imageService.DeleteImageAsync(oldImageUrl);

                existingMap.ImageUrl = await _imageService.UploadImageAsync(dto.ImageFile);
            }

            if (dto.ImageWidthPx.HasValue)
                existingMap.ImageWidthPx = dto.ImageWidthPx;

            if (dto.ImageHeightPx.HasValue)
                existingMap.ImageHeightPx = dto.ImageHeightPx;

            if (dto.WidthM.HasValue)
                existingMap.WidthM = dto.WidthM.Value;

            if (dto.HeightM.HasValue)
                existingMap.HeightM = dto.HeightM.Value;

            await _floorMapRepository.UpdateAsync(existingMap);

            return new FloorMapResponseDto
            {
                Id = existingMap.Id,
                Name = existingMap.Name,
                ImageUrl = existingMap.ImageUrl,
                ImageWidthPx = existingMap.ImageWidthPx,
                ImageHeightPx = existingMap.ImageHeightPx,
                WidthM = existingMap.WidthM,
                HeightM = existingMap.HeightM
            };
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var map = await _floorMapRepository.GetByIdAsync(id);
            if (map == null)
                return false;

            if (!string.IsNullOrEmpty(map.ImageUrl))
                await _imageService.DeleteImageAsync(map.ImageUrl);

            await _floorMapRepository.DeleteAsync(map);
            return true;
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
