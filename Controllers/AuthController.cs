using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IndoorLocalization.Models.Responses;

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
                return Created(string.Empty,
                   ApiResponse<object>.Ok(
                       new { user.Id, user.Username, user.Email },
                       "User registered successfully."
                   ));
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                var result = await _authManager.LoginAsync(dto);
                if (result == null)
                    return Unauthorized(ApiResponse<object>.Fail("Invalid username or password.", "INVALID_CREDENTIALS"));

                return Ok(ApiResponse<LoginResponseDto>.Ok(
                    result,
                    "Login successful"
                ));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message, "VALIDATION_ERROR"));
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto)
        {
            var result = await _authManager.RefreshTokenAsync(dto);
            if (result == null)
            {
                return Unauthorized(ApiResponse<object>.Fail("Invalid refresh token.", "UNAUTHORIZED"));
            }

            return Ok(ApiResponse<RefreshTokenResponseDto>.Ok(
                result,
                "Token refreshed successfully."
            ));
        }

        [HttpPost("otp/send")]
        [AllowAnonymous]
        public async Task<IActionResult> SendOtp([FromBody] OtpSendRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest(ApiResponse<object>.Fail("Email is required.", "VALIDATION_ERROR"));
            }

            var result = await _authManager.SendOtpAsync(request.Email);

            if(result is long errorCode && errorCode == -100)
            {
                return NotFound(ApiResponse<object>.Fail("User not found.", "USER_NOT_FOUND"));
            }

            return Ok(ApiResponse<object?>.Ok(null, "OTP code has been sent to the registered email address."));

        }

        [HttpPost("otp/verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Otp))
            {
                return BadRequest(ApiResponse<object>.Fail("Email and OTP code are required.", "INCOMPLETE_DATA"));
            }

            var result = await _authManager.VerifyOtpAsync(request.Email, request.Otp);

            if (result is long errorCode && errorCode < 0)
            {
                string errorMessage = errorCode switch
                {
                    -1 => "Invalid email or OTP code.",
                    -2 => "OTP code has already been used. Please request a new one.",
                    -3 => "OTP code has expired. Please request a new one.",
                    -4 => "Maximum attempts reached. Please request a new one.",
                    _ => "Authentication failed."
                };

                return Unauthorized(ApiResponse<object>.Fail(errorMessage, "OTP_VERIFICATION_FAILED"));
            }

            if (result is not LoginResponseDto loginResult)
            {
                return Unauthorized(ApiResponse<object>.Fail(
                    "Authentication failed.",
                    "OTP_VERIFICATION_FAILED"
                ));
            }

            return Ok(ApiResponse<LoginResponseDto>.Ok(
                loginResult,
                "OTP verified successfully."
            ));
        }
    }
}
