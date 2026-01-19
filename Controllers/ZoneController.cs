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
            return Ok(zones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var zone = await _zoneService.GetByIdAsync(id);
            if (zone == null)
                return NotFound(new { message = "Zone not found" });

            return Ok(zone);
        }

        [HttpGet("floormap/{floorMapId}")]
        public async Task<IActionResult> GetByFloorMap(long floorMapId)
        {
            var zones = await _zoneService.GetByFloorMapAsync(floorMapId);
            return Ok(zones);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ZoneCreateRequestDto dto)
        {
            var created = await _zoneService.CreateAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] ZoneUpdateRequestDto dto)
        {
            var success = await _zoneService.UpdateAsync(id, dto);
            if (!success)
                return NotFound(new { message = "Zone not found" });

            return Ok(new { message = "Zone updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var success = await _zoneService.DeleteAsync(id);
            if (!success)
                return NotFound(new { message = "Zone not found" });

            return Ok(new { message = "Zone deleted successfully" });
        }
    }
}
