using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class PartnersViewModel : IPageViewModel
    {
        private ObservableCollection<Partner> partners = new ObservableCollection<Partner>(
            ParterGenerator.GeneratePartners(100)
        );

        public string Name
        {
            get { return "Partnerzy"; }
        }

        public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                {
                    new NamedCommand("Dodaj partnera"), 
                    new NamedCommand("Usun partnera")
                };
            }
        }

        public ObservableCollection<Partner> Partners
        {
            get { return partners; }
        }
    }
}
