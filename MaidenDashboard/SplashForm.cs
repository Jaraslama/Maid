using System;
using System.Drawing;
using System.Windows.Forms;

namespace MaidenDashboard
{
    public class SplashForm : Form
    {
        private readonly SplashOverlay overlay;

        public event EventHandler? AnimationFinished
        {
            add => overlay.AnimationFinished += value;
            remove => overlay.AnimationFinished -= value;
        }

        public SplashForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            TopMost = true;
            ShowInTaskbar = false;
            ClientSize = new Size(520, 340);

            overlay = new SplashOverlay()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                SoundFilePath = "effect.wav"
            };
            Controls.Add(overlay);
        }
    }
}