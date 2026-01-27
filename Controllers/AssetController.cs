using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IndoorLocalization.Models.Responses;


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
            return Ok(ApiResponse<object>.Ok(assets, "Assets retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var asset = await _assetService.GetByIdAsync(id);
            if (asset == null)
                return NotFound(ApiResponse<object>.Fail("Asset not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object>.Ok(asset, "Asset retrieved successfully."));
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var asset = await _assetService.GetByNameAsync(name);
            if (asset == null)
                return NotFound(ApiResponse<object>.Fail("Asset not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object>.Ok(asset, "Asset retrieved successfully."));
        }

        [HttpGet("floormap/{floorMapId}")]
        public async Task<IActionResult> GetByFloorMap(long floorMapId)
        {
            var list = await _assetService.GetAssetsByFloorMapAsync(floorMapId);
            return Ok(ApiResponse<object>.Ok(list, "Assets for floor map retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AssetCreateRequestDto dto)
        {
            try
            {
                var created = await _assetService.CreateAsync(dto);
                return Ok(ApiResponse<object>.Ok(created, "Asset created successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message, "VALIDATION_ERROR"));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ApiResponse<object>.Fail(ex.Message, "ALREADY_EXISTS"));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNameAndColor(long id, [FromBody] AssetUpdateNameColorRequestDto dto)
        {
            var success = await _assetService.UpdateNameAndColorAsync(id, dto);
            if (!success)
                return NotFound(ApiResponse<object>.Fail("Asset not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object?>.Ok(null, "Asset updated successfully."));
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(long id, [FromBody] AssetUpdateStatusRequestDto dto)
        {
            var success = await _assetService.UpdateStatusAsync(id, dto);
            if (!success)
                return NotFound(ApiResponse<object>.Fail("Asset not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object?>.Ok(null, "Asset status updated successfully."));
        }

        [HttpPut("{id}/coordinates")]
        public async Task<IActionResult> UpdateCoordinates(long id, [FromBody] AssetUpdateCoordinatesRequestDto dto)
        {
            var success = await _assetService.UpdateCoordinatesAsync(id, dto);
            if (!success)
                return NotFound(ApiResponse<object>.Fail("Asset not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object?>.Ok(null, "Asset coordinates updated successfully."));
        }

        [HttpPut("{id}/floormap")]
        public async Task<IActionResult> UpdateFloorMap(long id, [FromBody] AssetUpdateFloorMapRequestDto dto)
        {
            var success = await _assetService.UpdateFloorMapAsync(id, dto);
            if (!success)
                return NotFound(ApiResponse<object>.Fail("Asset not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object?>.Ok(null, "Asset floor map updated successfully."));
        }

        [HttpGet("{id}/history/position")]
        public async Task<IActionResult> GetPositionHistory(long id)
        {
            var history = await _assetService.GetPositionHistoryAsync(id);
            return Ok(ApiResponse<object>.Ok(history, "Asset position history retrieved successfully."));
        }

        [HttpGet("{id}/history/zones")]
        public async Task<IActionResult> GetZoneHistory(long id)
        {
            var history = await _assetService.GetZoneHistoryAsync(id);
            return Ok(ApiResponse<object>.Ok(history, "Asset zone history retrieved successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _assetService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.Fail("Asset not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object?>.Ok(null, "Asset deleted successfully."));
        }
    }
}
