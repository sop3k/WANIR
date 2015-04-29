using System.Windows;

using NHibernate;
using WANIRPartners.DB;

namespace WANIRPartners
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void StartupHandler(object sender, System.Windows.StartupEventArgs e)
        {
            Elysium.Manager.Apply(this, Elysium.Theme.Light, Elysium.AccentBrushes.Blue, Elysium.AccentBrushes.Sky);
        }
    }
}
