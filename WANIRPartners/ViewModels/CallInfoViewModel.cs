using System;
using System.Collections.Generic;

using MvvmFoundation.Wpf;

using WANIRPartners.Utils;
using WANIRPartners.Models;

namespace WANIRPartners.ViewModels
{
    class CallInfoViewModel : ChildPageViewModel
    {
        public CallInfoViewModel(ChildPageViewModel parent, Partner partner)
            : base(parent.Parent)
        {
            Partner = partner;
        }

        override public String Name
        {
            get { return "INFORMACJE O ROZMOWIE"; }
        }

        override public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                {
                    new NamedCommand(Const.SAVE_CAPTION, new RelayCommand(Close)),
                    new NamedCommand(Const.CANCEL_CAPTION, new RelayCommand(Close))
                };
            }
        }

        public Partner Partner
        {
            get;
            private set;
        }
    }
}
