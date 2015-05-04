using System.Windows;

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
            Boostrapper.Initialize();
        }
    }
}
