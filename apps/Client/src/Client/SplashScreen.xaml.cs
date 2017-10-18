using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {        
        public SplashScreen()
        {
            Initialized += SplashScreen_Initialized;

            InitializeComponent();
        }

        private void SplashScreen_Initialized(object sender, EventArgs e)
        {
            var back = new BackgroundWorker();

            back.DoWork += (s, evt) =>
            {
                Thread.Sleep(500);
            };

            back.RunWorkerCompleted += (s, evt) =>
            {
                var main = new MainWindow();
                main.Show();
                Close();
            };

            back.RunWorkerAsync();
        }
    }
}
