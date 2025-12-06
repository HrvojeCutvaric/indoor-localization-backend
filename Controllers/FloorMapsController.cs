using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IndoorLocalization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorMapsController : ControllerBase
    {
        private readonly IFloorMapService _floorMapService;
        public FloorMapsController(IFloorMapService floorMapService)
        {
            _floorMapService = floorMapService;
        }

        // GET /api/floormaps
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var maps = await _floorMapService.GetAllAsync();
            return Ok(maps);
        }

        // GET /api/floormaps/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var map = await _floorMapService.GetByIdAsync(id);
            if (map == null)
                return NotFound(new { message = "Floor map not found" });

            return Ok(map);
        }

        // POST /api/floormaps
        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] FloorMapCreateRequestDto dto)
        {
            try
            {
                var createdMap = await _floorMapService.CreateAsync(dto);
                return Ok(createdMap);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT /api/floormaps/{id}
        [HttpPut("{id}")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] FloorMapUpdateRequestDto dto)
        {
            try
            {
                var updatedMap = await _floorMapService.UpdateAsync(id, dto);
                if (updatedMap == null)
                    return NotFound(new { message = "Floor map not found" });

                return Ok(updatedMap);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /api/floormaps/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _floorMapService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Floor map not found" });

            return Ok(new { message = "Floor map deleted successfully" });
        }
    }
}
