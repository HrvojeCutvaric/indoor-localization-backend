using System.Text.Json.Serialization;

namespace IndoorLocalization.Models.DTOs
{
    public class AssetMQTTPositionDto
    {
        [JsonPropertyName("id")]
        public long AssetId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("colorHex")]
        public string? ColorHex { get; set; }

        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }

        [JsonPropertyName("floorMapId")]
        public long FloorMapId { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("lastSync")]
        public DateTime Timestamp { get; set; }
    }
}
