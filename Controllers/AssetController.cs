using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IndoorLocalization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _assetService.GetAllAsync();
            return Ok(assets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var asset = await _assetService.GetByIdAsync(id);
            if (asset == null)
                return NotFound(new { message = "Asset not found" });

            return Ok(asset);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var asset = await _assetService.GetByNameAsync(name);
            if (asset == null)
                return NotFound(new { message = "Asset not found" });

            return Ok(asset);
        }

        [HttpGet("floormap/{floorMapId}")]
        public async Task<IActionResult> GetByFloorMap(long floorMapId)
        {
            var list = await _assetService.GetAssetsByFloorMapAsync(floorMapId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssetCreateRequestDto dto)
        {
            try
            {
                var created = await _assetService.CreateAsync(dto);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNameAndColor(long id, [FromBody] AssetUpdateNameColorRequestDto dto)
        {
            var success = await _assetService.UpdateNameAndColorAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Asset not found" });

            return Ok(new { message = "Asset updated successfully" });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] AssetUpdateStatusRequestDto dto)
        {
            var success = await _assetService.UpdateStatusAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Asset not found" });

            return Ok(new { message = "Status updated" });
        }

        [HttpPut("{id}/coordinates")]
        public async Task<IActionResult> UpdateCoordinates(long id, [FromBody] AssetUpdateCoordinatesRequestDto dto)
        {
            var success = await _assetService.UpdateCoordinatesAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Asset not found" });

            return Ok(new { message = "Coordinates updated" });
        }

        [HttpPut("{id}/floormap")]
        public async Task<IActionResult> UpdateFloorMap(long id, [FromBody] AssetUpdateFloorMapRequestDto dto)
        {
            var success = await _assetService.UpdateFloorMapAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Asset not found" });

            return Ok(new { message = "Floor map updated" });
        }

        [HttpGet("{id}/history/position")]
        public async Task<IActionResult> GetPositionHistory(long id)
        {
            var history = await _assetService.GetPositionHistoryAsync(id);
            return Ok(history);
        }

        [HttpGet("{id}/history/zones")]
        public async Task<IActionResult> GetZoneHistory(long id)
        {
            var history = await _assetService.GetZoneHistoryAsync(id);
            return Ok(history);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _assetService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = "Asset not found" });

            return Ok(new { message = "Asset deleted successfully" });
        }
    }
}
