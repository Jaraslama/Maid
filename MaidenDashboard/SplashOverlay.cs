using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace MaidenDashboard
{
    public class SplashOverlay : Panel
    {
        private string text = "Maiden";
        private float phaseClock = 0f;

        private const float FadeInDuration = 0.5f;
        private const float HoldDuration = 3f;
        private const float FadeOutDuration = 0.5f;

        private float _startFontSize = 32f;
        private string _soundFilePath = "";

        private Timer animationTimer;
        private SoundPlayer soundPlayer;

        // Public properties for design-time adjustment
        [System.ComponentModel.Category("Appearance")]
        [System.ComponentModel.Description("Font size when text appears centered")]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public float StartFontSize
        {
            get => _startFontSize;
            set => _startFontSize = value;
        }

        [System.ComponentModel.Category("Behavior")]
        [System.ComponentModel.Description("Path to sound file (.wav) to play when animation starts")]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        [System.ComponentModel.Editor("System.Windows.Forms.Design.FileNameEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))]
        public string SoundFilePath
        {
            get => _soundFilePath;
            set => _soundFilePath = value;
        }

        public event EventHandler? AnimationFinished;

        public SplashOverlay()
        {
            DoubleBuffered = true;
            BackColor = Color.White;

            soundPlayer = new SoundPlayer();

            animationTimer = new Timer();
            animationTimer.Interval = 30;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();
        }

        private void PlaySound()
        {
            if (string.IsNullOrEmpty(_soundFilePath))
                return;

            try
            {
                // If it's just a filename, look in the application directory
                string soundPath = _soundFilePath;
                if (!System.IO.Path.IsPathRooted(soundPath))
                {
                    soundPath = System.IO.Path.Combine(AppContext.BaseDirectory, soundPath);
                }

                if (System.IO.File.Exists(soundPath))
                {
                    soundPlayer.SoundLocation = soundPath;
                    soundPlayer.Play();
                }
            }
            catch
            {
                // Silently ignore if sound file fails to play
            }
        }

        private void AnimationTimer_Tick(object? sender, EventArgs e)
        {
            phaseClock += animationTimer.Interval / 1000f;

            // Play sound on first frame
            if (phaseClock <= (animationTimer.Interval / 1000f) * 1.5f)
            {
                PlaySound();
            }

            if (phaseClock >= FadeInDuration + HoldDuration + FadeOutDuration)
            {
                animationTimer.Stop();
                Invalidate(); // Keep rendering the final state
                AnimationFinished?.Invoke(this, EventArgs.Empty);
                return;
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            PointF centerPos = new PointF(Width / 2f, Height / 2f);

            float fontSize;
            PointF pos;
            float opacity;

            if (phaseClock < FadeInDuration)
            {
                // Phase 1: Fade in
                float t = phaseClock / FadeInDuration;
                opacity = t;
                fontSize = StartFontSize;
                pos = centerPos;
            }
            else if (phaseClock < FadeInDuration + HoldDuration)
            {
                // Phase 2: Hold in center
                opacity = 1f;
                fontSize = StartFontSize;
                pos = centerPos;
            }
            else
            {
                // Phase 3: Fade out
                float t = (phaseClock - FadeInDuration - HoldDuration) / FadeOutDuration;
                t = Math.Min(t, 1f);

                opacity = 1f - t;
                fontSize = StartFontSize;
                pos = centerPos;
            }

            using (Font font = new Font("Segoe UI", fontSize, FontStyle.Bold))
            using (Brush brush = new SolidBrush(Color.FromArgb((int)(opacity * 255), 90, 60, 200)))
            {
                SizeF size = g.MeasureString(text, font);
                float x = pos.X - size.Width / 2f;
                float y = pos.Y - size.Height / 2f;

                g.DrawString(text, font, brush, x, y);
            }
        }
    }
}
