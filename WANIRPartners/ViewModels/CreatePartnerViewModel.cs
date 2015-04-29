using System;
using System.Collections.Generic;

using MvvmFoundation.Wpf;

using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class CreatePartnerViewModel : ChildPageViewModel
    {
        public CreatePartnerViewModel(PageViewModel parent)
            : base(parent)
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
                    new NamedCommand(Const.SAVE_CAPTION, new RelayCommand(Close)),
                    new NamedCommand(Const.CANCEL_CAPTION, new RelayCommand(Close))
                };
            }
        }
    }
}
