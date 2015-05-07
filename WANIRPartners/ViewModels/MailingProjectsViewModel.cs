using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

using MvvmFoundation.Wpf;

using WANIRPartners.Models;
using WANIRPartners.Utils;

using NHibernate.Linq;

namespace WANIRPartners.ViewModels
{
    public class MailingProjectsViewModel: PageViewModel
    {
        public MailingProjectsViewModel(IViewController controller)
            : base(controller)
        {
            CurrentProjectView = ProjectsViews.AsQueryable().FirstOrDefault();
        }

        override public String ViewName
        {
            get { return Const.MAILING_PROJECTS_CAPTION; }
        }
        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                List<NamedCommand> cmds = new List<NamedCommand>{
                    new NamedCommand(Const.ADD_CAPTION, new RelayCommand(
                        () => ShowView(new CreateProjectViewModel(this, true)))),
                    new NamedCommand(Const.DELETE_CAPTION, new RelayCommand(
                        ShowDeleteProjectView,
                        () => CurrentProjectView != null))
                };

                if(CurrentProjectView != null)
                    cmds.AddRange(CurrentProjectView.Commands);

                return new ObservableCollection<NamedCommand>(cmds);
            }
        }

        public ObservableCollection<SingleProjectViewModel> ProjectsViews
        {
            get
            {
                return new ObservableCollection<SingleProjectViewModel>(
                    from project in Session.Query<Project>().Where(p => p.Mailing)
                    select new SingleProjectViewModel(this, project)
                );
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

        public void ShowDeleteProjectView()
        {
            var view = new DeleteProjectViewModel(this, CurrentProjectView.CurrentProject);
            ShowView(view);
            CurrentProjectView = ProjectsViews.FirstOrDefault();
        }

        public ICommand ChangeProjectViewCommand
        {
            get
            {
                return new RelayCommand<SingleProjectViewModel>(p =>
                {
                    CurrentProjectView = p;
                    ViewController.ChangePageCommand.Execute(this);
                });
            }
        }
        
        SingleProjectViewModel _currentProjectView;
    }
}
