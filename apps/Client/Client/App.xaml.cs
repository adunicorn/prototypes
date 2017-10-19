using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;

        }

        private void Current_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            var messageBoxResult = MessageBox.Show("Sorry, there was a serious error, do you want to kill Columbus?", "Application Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Current.Shutdown();
            }
        }
    }
}
