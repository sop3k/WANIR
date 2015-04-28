using System;
using System.Collections.Generic;

using MvvmFoundation.Wpf;

using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class CreatePartnerViewModel : ChildPageViewModel
    {
        public CreatePartnerViewModel()
            : base(null)
        {}

        override public String Name
        {
            get { return "NOWY PROJEKT"; }
        }

        override public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                {
                    new NamedCommand("Zapisz", new RelayCommand(Close)),
                    new NamedCommand("Anuluj", new RelayCommand(Close))
                };
            }
        }
    }
}
