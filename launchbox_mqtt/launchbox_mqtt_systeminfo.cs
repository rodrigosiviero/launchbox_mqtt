using System.Runtime;
using System.Text.Json;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace launchbox_mqtt
{
    public class MqttSystemInfoPublisher : ISystemEventsPlugin
    {
        private readonly MqttClientHelper _mqttClientHelper;
        private SettingsManager _settingsManager = new SettingsManager();
        private MqttSettings _settings;
        public MqttSystemInfoPublisher()
        {
            _settings = _settingsManager.LoadSettings();
            _mqttClientHelper = new MqttClientHelper(_settings.BrokerUrl, _settings.Port, "LaunchBoxSystemInfo", _settings.Username, _settings.EncryptedPassword);
        }

        private async Task PublishGameInfo()
        {
            await _mqttClientHelper.ConnectMqttClient();
            var mqttClient = _mqttClientHelper.GetMqttClient();

            var payload = new
            {
                TotalGames = PluginHelper.DataManager.GetAllGames()?.Length ?? 0,
                TotalPlatforms = PluginHelper.DataManager.GetAllPlatforms()?.Length ?? 0,
                BigBoxRunning = PluginHelper.StateManager?.IsBigBox,
                IsBigBoxInAttractMode = PluginHelper.StateManager?.IsBigBoxInAttractMode ?? false,
                IsBigBoxLocked = PluginHelper.StateManager?.IsBigBoxLocked ?? false,
                IsPremium = PluginHelper.StateManager?.IsPremium ?? false,
                BigBoxCurrentTheme = PluginHelper.StateManager?.BigBoxCurrentTheme
            };
            string jsonPayload = JsonSerializer.Serialize(payload);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("launchbox/systeminfo")
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

        public void OnEventRaised(string eventType)
        {
            _ = PublishGameInfo();
        }
    }
}
