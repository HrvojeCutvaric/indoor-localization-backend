using IndoorLocalization.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IndoorLocalization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private readonly IReportsService _reportService;

        public ReportsController(IReportsService reportService)
        {
            _reportService = reportService;
        }


        [HttpGet("assets/{assetId}/spaghetti")]
        public async Task<IActionResult> GetSpaghettiReport(long assetId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from > to)
                return BadRequest("'from' must be earlier than 'to'");

            var result = await _reportService.GetPositionHistoryByRangeAsync(assetId, from, to);

            return Ok(result);
        }

        [HttpGet("floormaps/{floorMapId}/heatmap")]
        public async Task<IActionResult> GetHeatmapReport(long floorMapId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from > to)
                return BadRequest("'from' must be earlier than 'to'");

            var result = await _reportService.GetPositionHistoryByFloorMapAsync(floorMapId, from, to);

            return Ok(result);
        }

        [HttpGet("zones/retention")]
        public async Task<IActionResult> GetZoneRetention([FromQuery] long? assetId, [FromQuery] long? zoneId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            if (from.HasValue && to.HasValue && from > to)
                return BadRequest("'from' must be earlier than 'to'");

            var result = await _reportService.GetZoneRetentionAsync(assetId, zoneId, from, to);

            return Ok(result);
        }
    }
}
