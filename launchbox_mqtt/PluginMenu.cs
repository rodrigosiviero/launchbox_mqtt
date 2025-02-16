using Unbroken.LaunchBox.Plugins;

namespace launchbox_mqtt
{
    public class MqttSettingsPlugin : ISystemMenuItemPlugin
    {
        public string Caption => "MQTT Settings";

        public bool IsEnabled => true;

        public bool ShowInLaunchBox => true;

        public bool ShowInBigBox => true;

        public bool AllowInBigBoxWhenLocked => true;

        public Image IconImage => Properties.Resources.mqtti;

        public void OnSelected()
        {
            using (var form = new SettingsForm())
            {
                form.ShowDialog();
            }
        }
    }
}
