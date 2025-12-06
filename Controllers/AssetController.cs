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

        // GET /api/assets
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _assetService.GetAllAsync();
            return Ok(assets);
        }

        // GET /api/assets/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var asset = await _assetService.GetByIdAsync(id);
            if (asset == null)
                return NotFound(new { message = "Asset not found" });

            return Ok(asset);
        }

        // GET /api/assets/name/{name}
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var asset = await _assetService.GetByNameAsync(name);
            if (asset == null)
                return NotFound(new { message = "Asset not found" });

            return Ok(asset);
        }

        // GET /api/assets/floormap/{floorMapId}
        [HttpGet("floormap/{floorMapId}")]
        public async Task<IActionResult> GetByFloorMap(long floorMapId)
        {
            var list = await _assetService.GetAssetsByFloorMapAsync(floorMapId);
            return Ok(list);
        }

        // POST /api/assets
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

        // PUT /api/assets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNameAndColor(long id, [FromBody] AssetUpdateNameColorRequestDto dto)
        {
            var success = await _assetService.UpdateNameAndColorAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Asset not found" });

            return Ok(new { message = "Asset updated successfully" });
        }

        // PUT /api/assets/{id}/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] AssetUpdateStatusRequestDto dto)
        {
            var success = await _assetService.UpdateStatusAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Asset not found" });

            return Ok(new { message = "Status updated" });
        }

        // PUT /api/assets/{id}/coordinates
        [HttpPut("{id}/coordinates")]
        public async Task<IActionResult> UpdateCoordinates(long id, [FromBody] AssetUpdateCoordinatesRequestDto dto)
        {
            var success = await _assetService.UpdateCoordinatesAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Asset not found" });

            return Ok(new { message = "Coordinates updated" });
        }

        // PUT /api/assets/{id}/floormap
        [HttpPut("{id}/floormap")]
        public async Task<IActionResult> UpdateFloorMap(long id, [FromBody] AssetUpdateFloorMapRequestDto dto)
        {
            var success = await _assetService.UpdateFloorMapAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Asset not found" });

            return Ok(new { message = "Floor map updated" });
        }

        // GET /api/assets/{id}/history/position
        [HttpGet("{id}/history/position")]
        public async Task<IActionResult> GetPositionHistory(long id)
        {
            var history = await _assetService.GetPositionHistoryAsync(id);
            return Ok(history);
        }

        // GET /api/assets/{id}/history/zones
        [HttpGet("{id}/history/zones")]
        public async Task<IActionResult> GetZoneHistory(long id)
        {
            var history = await _assetService.GetZoneHistoryAsync(id);
            return Ok(history);
        }

        // DELETE /api/assets/{id}
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
