namespace IndoorLocalization.Models.DTOs
{
    public class OtpVerifyRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
