using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

using MvvmFoundation.Wpf;

using WANIRPartners.Models;
using WANIRPartners.Utils;

using System.Windows;

namespace WANIRPartners.ViewModels
{
    public class ProjectsViewModel: PageViewModel
    {
        public ProjectsViewModel(IViewController controller)
            : base(controller)
        {
            CurrentProjectView = ProjectsViews[0];
        }

        override public String Name
        {
            get { return Const.PROJECTS_CAPTION; }
        }

        override public IEnumerable<NamedCommand> Commands
        {
            get
            {
                List<NamedCommand> cmds = new List<NamedCommand>{
                    new NamedCommand(Const.ADD_CAPTION, new RelayCommand(
                        () => ShowView(new CreateProjectViewModel(this))))
                };
                cmds.AddRange(CurrentProjectView.Commands);
                return cmds;
            }
        }

        public ObservableCollection<SingleProjectViewModel> ProjectsViews
        {
            get 
            {
                return new ObservableCollection<SingleProjectViewModel>
                {
                    new SingleProjectViewModel(this, new Project("PROJECT_1")),
                    new SingleProjectViewModel(this, new Project("PROJECT_2")),
                    new SingleProjectViewModel(this, new Project("PROJECT_3")),
                    new SingleProjectViewModel(this, new Project("PROJECT_4"))
                };
            }
        }

        public SingleProjectViewModel CurrentProjectView
        {
            get { return _currentProjectView; }
            private set
            {
                _currentProjectView = value;
                RaisePropertyChanged("CurrentProjectView");
            }
        }

        public ICommand ChangeProjectViewCommand
        {
            get
            {
                return new RelayCommand<SingleProjectViewModel>(p => { CurrentProjectView = p; });
            }
        }

        private SingleProjectViewModel _currentProjectView;
    }
}
