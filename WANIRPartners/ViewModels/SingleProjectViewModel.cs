using System.Collections.Generic;
using System.Collections.ObjectModel;

using MvvmFoundation.Wpf;

using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class SingleProjectViewModel : ChildPageViewModel
    {
        public SingleProjectViewModel(PageViewModel parent, Project prj)
            : base(parent)
        {
            _project = prj;
            _partners = new ObservableCollection<Partner>(_project.Partners);
        }

        override public string Name
        {
            get { return _project.Name; }
        }

        public Partner CurrentPartner 
        { 
            get; 
            set; 
        }

        override public IEnumerable<NamedCommand> Commands
        {
            get
            {
                return new List<NamedCommand>
                    {
                        new NamedCommand(Const.CALL_CAPTION, 
                            new RelayCommand(() => ShowView(new CallInfoViewModel(this, CurrentPartner))))
                    };
            }
        }

        #region Project Accesor
        public ObservableCollection<Partner> ProjectPartners
        {
            get { return _partners; }
        }
        #endregion

        private readonly Project _project;
        private ObservableCollection<Partner> _partners;
    }
}