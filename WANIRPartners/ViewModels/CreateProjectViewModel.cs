﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WANIRPartners.Models;
using WANIRPartners.Utils;

using MvvmFoundation.Wpf;

namespace WANIRPartners.ViewModels
{
    public class CreateProjectViewModel : ChildPageViewModel
    {
        public CreateProjectViewModel()
            : base(null)
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
                    new NamedCommand("Zapisz", new RelayCommand(Close)),
                    new NamedCommand("Anuluj", new RelayCommand(Close))
                };
            }
        }
    }
}