using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WANIRPartners.Models;
using WANIRPartners.Utils;

using MvvmFoundation.Wpf;

namespace WANIRPartners.ViewModels
{
    public class CreateProjectViewModel : ChildPageViewModel
    {
        public CreateProjectViewModel(PageViewModel parent)
            : base(parent)
        { }

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