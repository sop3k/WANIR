using System.Collections.Generic;
using System.Collections.ObjectModel;
using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class SingleProjectViewModel : IPageViewModel
    {
        public SingleProjectViewModel(Project prj)
        {
            _project = prj;
        }

        public string Name
        {
            get { return _project.Name; }
        }

        public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>();
            }
        }

        private readonly Project _project;
    }
}