using System.Text.Json;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;
using Unbroken.LaunchBox.Plugins.RetroAchievements;

namespace launchbox_mqtt
{
    public class MqttRetroAchievPublisher : IGameLaunchingPlugin
    {
        private readonly MqttClientHelper _mqttClientHelper;
        private SettingsManager _settingsManager = new SettingsManager();
        private MqttSettings _settings;
        public MqttRetroAchievPublisher()
        {
            _settings = _settingsManager.LoadSettings();
            _mqttClientHelper = new MqttClientHelper(_settings.BrokerUrl, _settings.Port, "LaunchBoxRetro", _settings.Username, _settings.EncryptedPassword);
        }

        private async Task PublishGameInfo(IGame? game = null)
        {
            await _mqttClientHelper.ConnectMqttClient();
            var mqttClient = _mqttClientHelper.GetMqttClient();

            var payload = new
            {
                creds = PluginHelper.RetroAchievementsApi.GetHasCredentials(),
                GetGameInfoWithProgress = PluginHelper.RetroAchievementsApi.GetGameInfoWithProgress(game),
                TotalPoints = PluginHelper.RetroAchievementsApi.GetRecentlyPlayedGames(5, 0),
                TotalAchievements = PluginHelper.RetroAchievementsApi.GetUserRankAndScore(),
                LastGamePlayed = PluginHelper.RetroAchievementsApi.GetUserSummary(5, 5)
            };
            string jsonPayload = JsonSerializer.Serialize(payload);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("launchbox/retroachievements")
                .WithPayload(jsonPayload)
                .WithRetainFlag()
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .Build();

            await mqttClient.PublishAsync(message);
        }

        public void OnDisposed()
        {
            _mqttClientHelper.GetMqttClient()?.Dispose();
        }

        public void OnInitialized()
        {
            async void PublishGameInfoAsync()
            {
                await PublishGameInfo();
            }

            PublishGameInfoAsync();
        }

        public void OnBeforeGameLaunching(IGame game, IAdditionalApplication app, IEmulator emulator)
        {
            System.Console.WriteLine("OnBeforeGameLaunching");
        }

        public void OnAfterGameLaunched(IGame game, IAdditionalApplication app, IEmulator emulator)
        {
            // Use async void to avoid blocking the main thread
            async void PublishGameInfoAsync()
            {
                await PublishGameInfo(game);
            }

            PublishGameInfoAsync();
        }

        public void OnGameExited()
        {
            System.Console.WriteLine("GameExit");
        }
    }
}
