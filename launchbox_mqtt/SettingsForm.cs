using System;
using System.Windows.Forms;

namespace launchbox_mqtt
{
    public partial class SettingsForm : Form
    {
        private readonly SettingsManager _settingsManager = new SettingsManager();
        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private TextBox textBox2;
        private Label label2;
        private MaskedTextBox maskedTextBox1;
        private Label label3;
        private TextBox textBox3;
        private Label label4;
        private TextBox ImageServerUrl;
        private TextBox ImageServerPort;
        private Label label5;
        private Label label6;
        private TextBox BaseImageDir;
        private Label label7;
        private readonly MqttSettings _settings;

        public SettingsForm()
        {
            InitializeComponent();
            _settings = _settingsManager.LoadSettings();
            textBox1.Text = _settings.BrokerUrl;
            textBox2.Text = _settings.Port.ToString();
            textBox3.Text = _settings.Username;
            maskedTextBox1.Text = SettingsManager.DecryptString(_settings.EncryptedPassword);
            BaseImageDir.Text = _settings.BaseImageDirectory;
            ImageServerUrl.Text = _settings.ImageServerUrl;
            ImageServerPort.Text = _settings.ImageServerPort.ToString();
        }


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            button1 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            textBox2 = new TextBox();
            label2 = new Label();
            maskedTextBox1 = new MaskedTextBox();
            label3 = new Label();
            textBox3 = new TextBox();
            label4 = new Label();
            ImageServerUrl = new TextBox();
            ImageServerPort = new TextBox();
            label5 = new Label();
            label6 = new Label();
            BaseImageDir = new TextBox();
            label7 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(407, 193);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "Save";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(38, 52);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(178, 23);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(38, 34);
            label1.Name = "label1";
            label1.Size = new Size(58, 15);
            label1.TabIndex = 2;
            label1.Text = "MQTT Url";
            label1.Click += label1_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(240, 52);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(61, 23);
            textBox2.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(240, 34);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 4;
            label2.Text = "MQTT Port";
            label2.Click += label2_Click;
            // 
            // maskedTextBox1
            // 
            maskedTextBox1.HidePromptOnLeave = true;
            maskedTextBox1.Location = new Point(172, 124);
            maskedTextBox1.Name = "maskedTextBox1";
            maskedTextBox1.Size = new Size(100, 23);
            maskedTextBox1.TabIndex = 5;
            maskedTextBox1.MaskInputRejected += maskedTextBox1_MaskInputRejected;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(172, 106);
            label3.Name = "label3";
            label3.Size = new Size(93, 15);
            label3.TabIndex = 6;
            label3.Text = "MQTT Password";
            label3.Click += label3_Click;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(38, 124);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(100, 23);
            textBox3.TabIndex = 7;
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(38, 106);
            label4.Name = "label4";
            label4.Size = new Size(66, 15);
            label4.TabIndex = 8;
            label4.Text = "MQTT User";
            label4.Click += label4_Click;
            // 
            // ImageServerUrl
            // 
            ImageServerUrl.Location = new Point(38, 193);
            ImageServerUrl.Name = "ImageServerUrl";
            ImageServerUrl.Size = new Size(178, 23);
            ImageServerUrl.TabIndex = 9;
            ImageServerUrl.TextChanged += ImageServerUrl_TextChanged;
            // 
            // ImageServerPort
            // 
            ImageServerPort.Location = new Point(240, 193);
            ImageServerPort.Name = "ImageServerPort";
            ImageServerPort.Size = new Size(61, 23);
            ImageServerPort.TabIndex = 10;
            ImageServerPort.TextChanged += textBox5_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(38, 175);
            label5.Name = "label5";
            label5.Size = new Size(90, 15);
            label5.TabIndex = 11;
            label5.Text = "ImageServer Url";
            label5.Click += label5_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(240, 175);
            label6.Name = "label6";
            label6.Size = new Size(97, 15);
            label6.TabIndex = 12;
            label6.Text = "ImageServer Port";
            // 
            // BaseImageDir
            // 
            BaseImageDir.Location = new Point(38, 256);
            BaseImageDir.Name = "BaseImageDir";
            BaseImageDir.Size = new Size(263, 23);
            BaseImageDir.TabIndex = 13;
            BaseImageDir.TextChanged += BaseImageDir_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(38, 229);
            label7.Name = "label7";
            label7.Size = new Size(118, 15);
            label7.TabIndex = 14;
            label7.Text = "Image Directory Path";
            label7.Click += label7_Click;
            // 
            // SettingsForm
            // 
            ClientSize = new Size(517, 291);
            Controls.Add(label7);
            Controls.Add(BaseImageDir);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(ImageServerPort);
            Controls.Add(ImageServerUrl);
            Controls.Add(label4);
            Controls.Add(textBox3);
            Controls.Add(label3);
            Controls.Add(maskedTextBox1);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(button1);
            HelpButton = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            Load += SettingsForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _settings.BrokerUrl = textBox1.Text;
            _settings.Port = int.Parse(textBox2.Text);
            _settings.Username = textBox3.Text;
            _settings.EncryptedPassword = SettingsManager.EncryptString(maskedTextBox1.Text);
            _settings.ImageServerUrl = ImageServerUrl.Text;
            _settings.ImageServerPort = int.Parse(ImageServerPort.Text);
            _settings.BaseImageDirectory = BaseImageDir.Text;
            _settingsManager.SaveSettings(_settings);
            MessageBox.Show("Settings saved. Restart LaunchBox for changes to take effect.");
            Close();
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void BaseImageDir_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void ImageServerUrl_TextChanged(object sender, EventArgs e)
        {

        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
