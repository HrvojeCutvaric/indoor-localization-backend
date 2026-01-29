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
    public class ZoneController : ControllerBase
    {
        private readonly IZoneService _zoneService;

        public ZoneController(IZoneService zoneService)
        {
            _zoneService = zoneService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var zones = await _zoneService.GetAllAsync();
            return Ok(ApiResponse<object>.Ok(zones, "Zones retrieved successfully."));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var zone = await _zoneService.GetByIdAsync(id);
            if (zone == null)
                return NotFound(ApiResponse<object>.Fail("Zone not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object>.Ok(zone, "Zone retrieved successfully."));
        }

        [HttpGet("floormap/{floorMapId}")]
        public async Task<IActionResult> GetByFloorMap(long floorMapId)
        {
            var zones = await _zoneService.GetByFloorMapAsync(floorMapId);
            return Ok(ApiResponse<object>.Ok(zones, "Zones for floor map retrieved successfully."));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ZoneCreateRequestDto dto)
        {
            var created = await _zoneService.CreateAsync(dto);
            return Ok(ApiResponse<object>.Ok(created, "Zone created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ZoneUpdateRequestDto dto)
        {
            var success = await _zoneService.UpdateAsync(id, dto);
            if (!success)
                return NotFound(ApiResponse<object>.Fail("Zone not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object?>.Ok(null, "Zone updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _zoneService.DeleteAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.Fail("Zone not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object?>.Ok(null, "Zone deleted successfully."));
        }
    }
}
