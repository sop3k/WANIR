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
            get { return Const.PROJECTS_CAPTION; }
        }

        override public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                {
                    new NamedCommand(Const.ADD_CAPTION, new RelayCommand(
                        () => ShowView(new CreateProjectViewModel(this))))
                };
            }
        }

        public ObservableCollection<PageViewModel> ProjectViewModels
        {
            get { return projects; }
        }
    }
}
