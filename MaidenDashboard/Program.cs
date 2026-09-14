using System;
using System.Windows.Forms;

namespace MaidenDashboard
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SplashApplicationContext());
        }
    }

    class SplashApplicationContext : ApplicationContext
    {
        private readonly SplashForm splash;

        public SplashApplicationContext()
        {
            splash = new SplashForm();
            splash.AnimationFinished += OnSplashFinished;
            splash.Show();
        }

        private void OnSplashFinished(object? sender, EventArgs e)
        {
            splash.Close();

            var dashboard = new DashboardForm();
            dashboard.FormClosed += (_, _) => ExitThread();
            dashboard.Show();
        }
    }
}