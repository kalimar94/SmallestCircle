using System.Windows;
using System.Windows.Threading;

namespace SmallestCircle.Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += OnException;
        }

        void OnException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"An unhandled exception occurred: { e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
