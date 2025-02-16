using MQTTnet;
using MQTTnet.Client;
using System;
using System.Threading.Tasks;

namespace launchbox_mqtt
{
    public class MqttClientHelper
    {
        private readonly IMqttClient _mqttClient;
        private readonly MqttClientOptions _mqttOptions;

        public MqttClientHelper(string brokerAddress, int brokerPort, string clientId, string username, string password)
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            _mqttOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(brokerAddress, brokerPort)
                .WithCredentials(username, SettingsManager.DecryptString(password))
                .WithClientId(clientId)
                .WithKeepAlivePeriod(TimeSpan.FromSeconds(60)) // Set keep-alive interval
                .WithCleanSession()
                .Build();

            _mqttClient.DisconnectedAsync += async e =>
            {
                Console.WriteLine("Disconnected from MQTT broker. Reconnecting...");
                await Task.Delay(TimeSpan.FromSeconds(5)); // Wait before reconnecting
                await ConnectMqttClient();
            };
        }

        public async Task ConnectMqttClient()
        {
            if (!_mqttClient.IsConnected)
            {
                try
                {
                    await _mqttClient.ConnectAsync(_mqttOptions);
                    Console.WriteLine("Connected to MQTT broker.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"MQTT connection failed: {ex.Message}");
                }
            }
        }

        public IMqttClient GetMqttClient()
        {
            return _mqttClient;
        }

        public async Task SubscribeToTopic(string topic, Func<MqttApplicationMessageReceivedEventArgs, Task> messageHandler)
        {
            _mqttClient.ApplicationMessageReceivedAsync += messageHandler;
            await _mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder().WithTopicFilter(topic).Build());
        }
    }
}
