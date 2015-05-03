using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MvvmFoundation.Wpf;
using WANIRPartners.Utils;
using WANIRPartners.Models;

namespace WANIRPartners.ViewModels
{
    public class CallInfoViewModel : CommonChildPageViewModel<CallInfo>
    {       
        public CallInfoViewModel(ChildPageViewModel parent, Project project, PartnerInfoCall call) 
            : base(parent.Parent)
        {
            Partner = call.Partner;
            Project = project;
            CallInfo = call.CallInfo;
         
            _singleProjectView = (SingleProjectViewModel)parent;
        }
        #region View
        override public String ViewName
        {
            get { return "INFORMACJE O ROZMOWIE"; }
        }
        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                    new NamedCommand(Const.SAVE_CAPTION, new RelayCommand(Save)),
                    new NamedCommand(Const.CANCEL_CAPTION, new RelayCommand(Close))
                };
            }
        }
        #endregion
        public CallInfo CallInfo { get; set; }
        public Partner Partner{ get; private set; }
        public Project Project{ get; private set; }
        public ICommand MoveToNextCommand {
            get { return new RelayCommand(MoveToNextPartner); }
        }

        void MoveToNextPartner()
        {
            Save();
            
            var info = _singleProjectView.PartnersWithCallInfo
                .SkipWhile(x => x.CallInfo != null)
                .FirstOrDefault();

            ShowView(new CallInfoViewModel(_singleProjectView, Project, info));
        }

        private readonly SingleProjectViewModel _singleProjectView;
    }
}
