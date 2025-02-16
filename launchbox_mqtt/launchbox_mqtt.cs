using System.IO;
using System.Text.Json;
using MQTTnet;
using MQTTnet.Protocol;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace launchbox_mqtt
{
    public class MqttGamePublisher : IGameLaunchingPlugin
    {
        private readonly MqttClientHelper _mqttClientHelper;
        private SettingsManager _settingsManager = new SettingsManager();
        private MqttSettings _settings;

        public MqttGamePublisher()
        {
            _settings = _settingsManager.LoadSettings();
            _mqttClientHelper = new MqttClientHelper(_settings.BrokerUrl, _settings.Port, "LaunchBoxNowPlaying", _settings.Username, _settings.EncryptedPassword);
        }

        public static string GetImageUrl(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return string.Empty;
            var settingsManager = new SettingsManager();
            var settings = settingsManager.LoadSettings();
            string BaseUrl = settings.ImageServerUrl + ":" + settings.ImageServerPort + "/";
            string BaseDirectory = settings.BaseImageDirectory;

            return $"{BaseUrl}{Path.GetRelativePath(BaseDirectory, imagePath).Replace("\\", "/")}";
        }

        private async Task PublishGameInfo(IGame? game, string status)
        {
            var settingsManager = new SettingsManager();
            var settings = settingsManager.LoadSettings();
            string baseDirectory = settings.BaseImageDirectory;

            string BackgroundImagePath = GetImageUrl(game?.BackgroundImagePath);
            string Cart3DImagePath = GetImageUrl(game?.Cart3DImagePath);
            string CartBackImagePath = GetImageUrl(game?.CartBackImagePath);
            string CartFrontImagePath = GetImageUrl(game?.CartFrontImagePath);
            string ClearLogoImagePath = GetImageUrl(game?.ClearLogoImagePath);
            string BackImagePath = GetImageUrl(game?.BackImagePath);
            string Box3DImagePath = GetImageUrl(game?.Box3DImagePath);
            string FrontImagePath = GetImageUrl(game?.FrontImagePath);
            string MarqueeImagePath = GetImageUrl(game?.MarqueeImagePath);
            string PlatformClearLogoImagePath = GetImageUrl(game?.PlatformClearLogoImagePath);
            string ScreenshotImagePath = GetImageUrl(game?.ScreenshotImagePath);

            await _mqttClientHelper.ConnectMqttClient();
            var mqttClient = _mqttClientHelper.GetMqttClient();

            var payload = new
            {
                Title = game?.Title ?? "Unknown",
                Platform = game?.Platform ?? "Unknown",
                Developer = game?.Developer ?? "Unknown",
                Publisher = game?.Publisher ?? "Unknown",
                ReleaseDate = game?.ReleaseDate,
                Rating = game?.Rating ?? "Unknown",
                LastPlayedDate = game?.LastPlayedDate,
                Genres = game?.Genres?.ToArray() ?? Array.Empty<string>(),
                Favorite = game?.Favorite ?? false,
                PlayCount = game?.PlayCount ?? 0,
                Image = new
                {
                    BackgroundImagePath = BackgroundImagePath,
                    Cart3DImagePath = Cart3DImagePath,
                    CartBackImagePath = CartBackImagePath,
                    CartFrontImagePath = CartFrontImagePath,
                    ClearLogoImagePath = ClearLogoImagePath,
                    BackImagePath = BackImagePath,
                    Box3DImagePath = Box3DImagePath,
                    FrontImagePath = FrontImagePath,
                    MarqueeImagePath = MarqueeImagePath,
                    PlatformClearLogoImagePath = PlatformClearLogoImagePath,
                    ScreenshotImagePath = ScreenshotImagePath
                },
                Status = status
            };

            string jsonPayload = JsonSerializer.Serialize(payload);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("launchbox/nowplaying")
                .WithPayload(jsonPayload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await mqttClient.PublishAsync(message);
        }

        public void OnDisposed()
        {
            _mqttClientHelper.GetMqttClient()?.Dispose();
        }

        public void OnBeforeGameLaunching(IGame game, IAdditionalApplication app, IEmulator emulator)
        {
            
            async void PublishGameInfoAsync()
            {
                await PublishGameInfo(game, "Launching");
            }

            PublishGameInfoAsync();

        }

        public async void OnAfterGameLaunched(IGame game, IAdditionalApplication app, IEmulator emulator)
        {
            
            async void PublishGameInfoAsync()
            {
                await PublishGameInfo(game, "Playing");
            }

            PublishGameInfoAsync();
        }

        public async void OnGameExited()
        {
            await PublishGameInfo(null, "Stopped");
        }
    }
}
