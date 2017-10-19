using System.Threading;
using Client.ViewModels;
using MahApps.Metro.Controls;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindowViewModel ViewModel { get; set; } = new MainWindowViewModel();

        public MainWindow()
        {
            ThreadPool.SetMinThreads(100, 100);

            ViewModel = new MainWindowViewModel();

            InitializeComponent();

            DataContext = ViewModel;
        }

        private void Settings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            settingsFlyout.IsOpen = true;
        }
    }
}
