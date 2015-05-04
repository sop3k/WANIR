using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WANIRPartners.Models;
using WANIRPartners.Utils;

using MvvmFoundation.Wpf;

namespace WANIRPartners.ViewModels
{
    class CreateProjectViewModel : CommonChildPageViewModel<Project>
    {
        public CreateProjectViewModel(PageViewModel parent)
            : base(parent)
        { }

        override public String ViewName
        {
            get { return Const.PROJECT_CREATE_CAPTION; }
        }

        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                    new NamedCommand(Const.SAVE_CAPTION, new RelayCommand(Save)),
                    new NamedCommand(Const.CANCEL_CAPTION, new RelayCommand(Close))
                };
            }
        }

        public bool Mailing{ get; set; }
    }
}