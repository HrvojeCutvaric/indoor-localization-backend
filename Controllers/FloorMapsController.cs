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
    public class FloorMapsController : ControllerBase
    {
        private readonly IFloorMapService _floorMapService;
        public FloorMapsController(IFloorMapService floorMapService)
        {
            _floorMapService = floorMapService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var maps = await _floorMapService.GetAllAsync();
            return Ok(ApiResponse<object>.Ok(maps, "Floor maps retrieved successfully."));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var map = await _floorMapService.GetByIdAsync(id);
            if (map == null)
                return NotFound(ApiResponse<object>.Fail("Floor map not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object>.Ok(map, "Floor map retrieved successfully."));
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] FloorMapCreateRequestDto dto)
        {
            try
            {
                var createdMap = await _floorMapService.CreateAsync(dto);
                return Ok(ApiResponse<object>.Ok(createdMap, "Floor map created successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message, "VALIDATION_ERROR"));
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] FloorMapUpdateRequestDto dto)
        {
            try
            {
                var updatedMap = await _floorMapService.UpdateAsync(id, dto);
                if (updatedMap == null)
                    return NotFound(ApiResponse<object>.Fail("Floor map not found.", "NOT_FOUND"));

                return Ok(ApiResponse<object>.Ok(updatedMap, "Floor map updated successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message, "VALIDATION_ERROR"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _floorMapService.DeleteAsync(id);

            if (!deleted)
                return NotFound(ApiResponse<object>.Fail("Floor map not found.", "NOT_FOUND"));

            return Ok(ApiResponse<object?>.Ok(null, "Floor map deleted successfully."));
        }
    }
}
