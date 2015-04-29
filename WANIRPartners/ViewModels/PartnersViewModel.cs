using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using MvvmFoundation.Wpf;

using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class PartnersViewModel : PageViewModel
    {
        public PartnersViewModel(IViewController controller)
            : base(controller)
        { }

        private ObservableCollection<Partner> partners = new ObservableCollection<Partner>(
            ParterGenerator.GeneratePartners(100, "Partner")
        );

        override public string Name
        {
            get { return Const.PARTNERS_CAPTION; }
        }

        override public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                {
                    new NamedCommand(Const.ADD_CAPTION, new RelayCommand(AddPartnerCommand)), 
                    new NamedCommand(Const.DELETE_CAPTION, null)
                };
            }
        }

        public ObservableCollection<Partner> Partners
        {
            get { return partners; }
        }

        private void AddPartnerCommand()
        {
            ShowView(new CreatePartnerViewModel(this));
        }
    }
}
