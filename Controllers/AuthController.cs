using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Models.Entities;
using IndoorLocalization.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IndoorLocalization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            try
            {
                var user = await _userService.RegisterAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    message = "Registration successful",
                    user = new
                    {
                        user.Id,
                        user.Username,
                        user.Email,
                        user.FirstName,
                        user.LastName
                    }
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred during registration." });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenRequest)
        {
            if(tokenRequest is null || 
               string.IsNullOrEmpty(tokenRequest.AccessToken) || 
               string.IsNullOrEmpty(tokenRequest.RefreshToken))
            {
                 return BadRequest("Invalid client request");
            }

            var principal = _jwtService.GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
            if(principal is null) return BadRequest("Invalid access token or refresh token 1");

            var userEmailClaim = principal.Claims.FirstOrDefault(c =>
                c.Type.Equals("email", StringComparison.OrdinalIgnoreCase) ||
                c.Type == ClaimTypes.Email
                )?.Value;
            if (string.IsNullOrEmpty(userEmailClaim)) return BadRequest("Invalid access token or refresh token 2");

            var user = await _userService.GetByEmailAsync(userEmailClaim);
            if(user is null || 
               user.RefreshToken != tokenRequest.RefreshToken || 
               user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("Invalid access token or refresh token 3");
            }

            var newAccessToken = _jwtService.CreateToken(user);
            var newRefreshToken = _jwtService.RefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userService.UpdateTokenAsync(user);

            return Ok(new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
