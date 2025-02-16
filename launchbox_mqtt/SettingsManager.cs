using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace launchbox_mqtt
{
    public class SettingsManager
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "mqtt_settings.json");

        public MqttSettings LoadSettings()
        {
            if (!File.Exists(ConfigPath)) return new MqttSettings();
            string json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<MqttSettings>(json);
        }

        public void SaveSettings(MqttSettings settings)
        {
            string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigPath, json);
        }

        public static string EncryptString(string plainText)
        {
            byte[] data = Encoding.UTF8.GetBytes(plainText);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptString(string encryptedText)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encryptedText);
                byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch
            {
                return "";
            }
        }
    }

    public class MqttSettings
    {
        public string BrokerUrl { get; set; } = "192.168.1.100";
        public int Port { get; set; } = 1883;
        public string Username { get; set; } = "";
        public string EncryptedPassword { get; set; } = "";
        public string ImageServerUrl { get; set; } = "http://192.168.1.101";
        public int ImageServerPort { get; set; } = 8089;
        public string BaseImageDirectory { get; set; } = $"C:/Users/{Environment.UserName}/LaunchBox/Images/";
    }
}
