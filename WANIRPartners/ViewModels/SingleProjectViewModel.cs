﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class SingleProjectViewModel : PageViewModel
    {
        public SingleProjectViewModel(IViewController controller, Project prj)
            : base(controller)
        {
            _project = prj;
        }

        override public string Name
        {
            get { return _project.Name; }
        }

        override public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>();
            }
        }

        private readonly Project _project;
    }
}