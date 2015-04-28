using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using MvvmFoundation.Wpf;

using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class ProjectsViewModel: PageViewModel
    {
        public ProjectsViewModel(IViewController controller)
            : base(controller)
        { }

        private ObservableCollection<PageViewModel> projects = new ObservableCollection<PageViewModel>();

        override public String Name
        {
            get { return "Projekty"; }
        }

        override public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                {
                    new NamedCommand("Dodaj projekt", new RelayCommand(AddProjectCommand)),
                };
            }
        }

        public ObservableCollection<PageViewModel> ProjectViewModels
        {
            get { return projects; }
        }

        private void AddProjectCommand()
        {
            CreateProjectViewModel view = new CreateProjectViewModel();
            ICommand change = ViewController.ChangePageCommand;
            change.Execute(view);
        }
    }
}
