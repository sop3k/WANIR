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
            ParterGenerator.GeneratePartners(100)
        );

        override public string Name
        {
            get { return "Partnerzy"; }
        }

        override public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                {
                    new NamedCommand("Dodaj partnera", new RelayCommand(AddPartnerCommand)), 
                    new NamedCommand("Usun partnera", null)
                };
            }
        }

        public ObservableCollection<Partner> Partners
        {
            get { return partners; }
        }

        private void AddPartnerCommand()
        {
            CreatePartnerViewModel view = new CreatePartnerViewModel();
            ICommand change = ViewController.ChangePageCommand;
            change.Execute(view);
        }
    }
}
