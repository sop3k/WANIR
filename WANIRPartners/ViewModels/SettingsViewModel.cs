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

        override public string ViewName
        {
            get { return Const.SETTINGS_CAPTION; }
        }

        public string CallReportPrintTemplatePath
        {
            get { return Settings.CallReportPrintTemplatePath; }
            set { Settings.CallReportPrintTemplatePath = value; }
        }

        public string PartnerPrintTemplatePath
        {
            get { return Settings.PartnerPrintTemplatePath; }
            set { Settings.PartnerPrintTemplatePath = value; }
        }

        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                    new NamedCommand(Const.SAVE_CAPTION, null)
                };
            }
        }
    }
}