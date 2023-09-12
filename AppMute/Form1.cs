using AppMute.Properties;
using Microsoft.Win32;
using NAudio.CoreAudioApi;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AppMute
{
    public partial class Form1 : Form
    {
        private string AppName = "FMute";
        private MMDevice device;

        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(50, 50);
            this.MinimumSize = new Size(50, 50);
            this.MaximumSize = new Size(50, 50);

            this.StartPosition = FormStartPosition.Manual;
            int x = Screen.PrimaryScreen.WorkingArea.Left;
            int y = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
            this.Location = new Point(x, y);

            using (var enumerator = new NAudio.CoreAudioApi.MMDeviceEnumerator())
            {
                device = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
            }

            SetStartup();

            checkMute_unmute();

            SendNotification("FMute is running");
        }

        private void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            rk.SetValue(AppName, Application.ExecutablePath);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            mute_unmute();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch(e.KeyCode)
                {
                    case Keys.M:

                        mute_unmute();

                        break;
                }
            }
        }

        private void mute_unmute()
        {
            if (device.AudioEndpointVolume.Mute)
            {
                device.AudioEndpointVolume.Mute = false; //or false
                pictureBox1.Image = Resources.mute;
                SendNotification("Unmuted");
            }

            else
            {
                device.AudioEndpointVolume.Mute = true;
                pictureBox1.Image = Resources.unmute;
                SendNotification("Muted");
            }
        }

        private void checkMute_unmute()
        {
            if (device.AudioEndpointVolume.Mute)
            {
                pictureBox1.Image = Resources.unmute;
            }

            else
            {
                pictureBox1.Image = Resources.mute;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            device.AudioEndpointVolume.Mute = false;

            Debug.WriteLine(" > UNMUTE");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            device.AudioEndpointVolume.Mute = false;

            Debug.WriteLine(" > UNMUTE");
        }

        private void SendNotification(string text)
        {
        }
    }
}
