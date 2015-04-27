using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class CreateProjectViewModel : IPageViewModel
    {
        public String Name
        {
            get { return "NOWY PROJEKT"; }
        }

        public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                {
                    new NamedCommand("Zapisz"),
                    new NamedCommand("Anuluj")
                };
            }
        }
    }
}