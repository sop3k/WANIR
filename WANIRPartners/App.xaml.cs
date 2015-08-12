using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;

using NHibernate;
using WANIRPartners.Utils;

namespace WANIRPartners
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void StartupHandler(object sender, System.Windows.StartupEventArgs e)
        {
#if !DEBUG
            DispatcherUnhandledException +=
                new DispatcherUnhandledExceptionEventHandler(
                    OnAppDispatcherUnhandledException);
#endif
            Boostrapper.Initialize();
        }

        void OnAppDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Catch all exceptions
            var errorView = new Views.ErrorView();
            
            errorView.DataContext = new ViewModels.ErrorViewModel(e.Exception);
            errorView.Show();

            e.Handled = true;
        }
    }
}
