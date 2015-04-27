using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class ProjectsViewModel : IPageViewModel
    {
        private ObservableCollection<IPageViewModel> projects = new ObservableCollection<IPageViewModel>
        {
            new SingleProjectViewModel(new Project("Projekt_1")),
            new SingleProjectViewModel(new Project("Projekt_2")),
            new SingleProjectViewModel(new Project("Projekt_3")),
            new SingleProjectViewModel(new Project("Projekt_4"))
        };

        public String Name
        {
            get { return "Projekty"; }
        }

        public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                {
                    new NamedCommand("Dodaj projekt"), 
                    new NamedCommand("Usun projekt")
                };
            }
        }

        public ObservableCollection<IPageViewModel> ProjectViewModels
        {
            get { return projects; }
        }
    }
}
