using System.Collections.Generic;
using System.Collections.ObjectModel;

using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class SettingsViewModel : PageViewModel
    {
        public SettingsViewModel(IViewController controller)
            : base(controller)
        { }

        override public string Name
        {
            get { return Const.SETTINGS_CAPTION; }
        }

        override public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                {
                    new NamedCommand("Zapisz", null)
                };
            }
        }
    }
}