﻿using System;
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
        public CreateProjectViewModel(PageViewModel parent, bool isMailing)
            : base(parent)
        {
            Mailing = isMailing;
        }

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

        public bool Mailing
        {
            get;
            private set;
        }

        public bool? Cooperation
        {
            get;
            set;
        }

        public string CooperationConverter
        {
            get { return Const.Convert(Cooperation); }
            set { Cooperation = Const.ConvertToBool(value); }
        }
    }
}