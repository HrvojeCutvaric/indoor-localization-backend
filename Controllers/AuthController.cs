using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndoorLocalization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            try
            {
                var user = await _authManager.RegisterAsync(dto);
                return Created("", new { user.Id, user.Username, user.Email });
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                var result = await _authManager.LoginAsync(dto);
                if (result == null)
                    return Unauthorized(new { message = "Invalid credentials" });

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            var result = await _authManager.RefreshTokenAsync(dto);
            if (result == null)
                return Unauthorized(new { message = "Invalid refresh token" });

            return Ok(result);
        }

        [HttpPost("otp/send")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOtp([FromBody] OtpSendRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            var result = await _authManager.SendOtpAsync(request.Email);

            if(result is long errorCode && errorCode < 0)
            {
                if(errorCode == -100)
                {
                    return NotFound(new { message = "User not found.", errorCode = errorCode });
                }
            }

            return Ok(new { message = "OTP code has been sent to the registered email address." });

        }

        [HttpPost("otp/verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Otp))
            {
                return BadRequest(new { message = "Email and OTP code are required." });
            }

            var result = await _authManager.VerifyOtpAsync(request.Email, request.Otp);

            if (result is long errorCode && errorCode < 0)
            {
                string errorMessage = errorCode switch
                {
                    -1 => "Invalid email or OTP code.",
                    -2 => "OTP code has already been used. Please request a new code.",
                    -3 => "OTP code has expired. Please request a new code.",
                    -4 => "Maximum attempts reached. Please request a new code.",
                    _ => "An unexpected authentication error occurred."
                };

                return Unauthorized(new { message = errorMessage, errorCode = errorCode });
            }

            if (result == null || result is not LoginResponseDto)
            {
                return Unauthorized(new { message = "Authentication failed. Result type mismatch." });
            }
            return Ok(result);
        }
    }
}
