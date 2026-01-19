using IndoorLocalization.Services.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;
using System.Text;
using IndoorLocalization.Models.DTOs;
using IndoorLocalization.Services;

namespace IndoorLocalization.Mqtt
{
    public class AssetPositionMqttListener : BackgroundService
    {
        private readonly IMqttClient _client;
        private readonly ILogger<AssetPositionMqttListener> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private const string TopicName = "air/assets/updates";

        public AssetPositionMqttListener(IServiceScopeFactory scopeFactory, ILogger<AssetPositionMqttListener> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _client = new MqttFactory().CreateMqttClient();
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("broker.hivemq.com", 1883)
                .WithCleanSession()
                .Build();

            _client.ApplicationMessageReceivedAsync += OnMqttMessageReceived;

            await _client.ConnectAsync(options, cancellationToken);
            await _client.SubscribeAsync(TopicName);
            
            _logger.LogInformation("AssetPositionMqttListener started and subscribed to topic {TopicName}", TopicName);
        }

        private async Task OnMqttMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            using var scope = _scopeFactory.CreateScope();
            var assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
            var floorMapService = scope.ServiceProvider.GetRequiredService<IFloorMapService>();
            var zoneDetectionService = scope.ServiceProvider.GetRequiredService<IZoneDetectionService>();

            try
            {
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                _logger.LogInformation("Received MQTT message: {payload}", payload);

                var assetPositionDto = JsonSerializer.Deserialize<AssetMQTTPositionDto>(payload);

                if (assetPositionDto == null)
                {
                    _logger.LogWarning("Invalid MQTT message received: {payload}", payload);
                    return;
                }

                if (!await ValidateMqttPayloadAsync(assetPositionDto, assetService, floorMapService)) return;
                    

                _logger.LogInformation(
                    "MQTT Asset Position => ID={Id}, X={X}, Y={Y}, Floor={Floor}, Time={Time}",
                    assetPositionDto.AssetId, assetPositionDto.X, assetPositionDto.Y, assetPositionDto.FloorMapId, assetPositionDto.Timestamp
                );

                var updateAssetCoordinates = new AssetUpdateCoordinatesRequestDto
                {
                    X = assetPositionDto.X,
                    Y = assetPositionDto.Y
                };

                await assetService.UpdateCoordinatesAsync(assetPositionDto.AssetId, updateAssetCoordinates);

                await assetService.AddPositionHistoryAsync(
                    assetPositionDto.AssetId,
                    assetPositionDto.X,
                    assetPositionDto.Y,
                    assetPositionDto.FloorMapId,
                    assetPositionDto.Timestamp
                );

                await zoneDetectionService.ProcessAssetPositionAsync(
                    assetPositionDto.AssetId,
                    assetPositionDto.X,
                    assetPositionDto.Y,
                    assetPositionDto.FloorMapId,
                    assetPositionDto.Timestamp
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing MQTT message.");
            }
        }

        private async Task<bool> ValidateMqttPayloadAsync(AssetMQTTPositionDto dto, IAssetService assetService, IFloorMapService floorMapService)
        {
            var asset = await assetService.GetByIdAsync(dto.AssetId);
            if (asset == null)
            {
                _logger.LogWarning("MQTT asset ignored — asset with ID {Id} does not exist.", dto.AssetId);
                return false;
            }

            var floorMap = await floorMapService.GetByIdAsync(dto.FloorMapId);
            if (floorMap == null)
            {
                _logger.LogWarning("MQTT asset ignored — floor map {FloorMapId} does not exist.", dto.FloorMapId);
                return false;
            }

            return true;
        }

    }
}
