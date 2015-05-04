using System;
using System.Collections.ObjectModel;
using MvvmFoundation.Wpf;
using WANIRPartners.Utils;
using WANIRPartners.Models;

namespace WANIRPartners.ViewModels
{
    public class DeleteProjectViewModel : CommonChildPageViewModel<Project>
    {
        public DeleteProjectViewModel(PageViewModel parent, Project project)
            : base(parent)
        {
            Project = project;
        }
        override public String ViewName
        {
            get { return Const.REMOVE_PROJECT; }
        }
        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                    new NamedCommand(Const.YES_CAPTION, new RelayCommand(Delete)),
                    new NamedCommand(Const.NO_CAPTION, new RelayCommand(Close))
                };
            }
        }
        public Project Project { get; set; }
        private void Delete()
        {
            using (var tx = Session.BeginTransaction())
            {
                Session.Delete(Project);
                tx.Commit();
            }

            Close();
        }
    }
}