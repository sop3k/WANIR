using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using MvvmFoundation.Wpf;

using WANIRPartners.Utils.Doc;
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

            if (call.CallInfo == null)
                CallInfo = new CallInfo { Partner = Partner, Project = Project };
            else
                CallInfo = call.CallInfo;
         
            _singleProjectView = (SingleProjectViewModel)parent;
            _next = _singleProjectView.PartnersWithCallInfo
                .SkipWhile(x => x.CallInfo != null)
                .FirstOrDefault();
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

        public DateTime FirstDate 
        {
            get
            {
                return CallInfo.FirstDate ?? DateTime.Now;
            }
            set
            {
                CallInfo.FirstDate = value;
            }
        }

        public DateTime SecondDate
        {
            get
            {
                return CallInfo.SecondDate ?? DateTime.Now;
            }
            set
            {
                CallInfo.SecondDate = value;
            }
        }

        public DateTime MeetingDate
        {
            get
            {
                return CallInfo.MeetingDate ?? DateTime.Now;
            }
            set
            {
                CallInfo.MeetingDate = value;
            }
        }

        public string OfferDate
        {
            get
            {
                if (Offer)
                    return CallInfo.OfferDate.ToString();
                else
                    return Const.NOT_SET;
            }
        }

        public bool Offer
        {
            get { return CallInfo.Offer; }
            set
            {
                CallInfo.Offer = value;
                RaisePropertyChanged("Offer");
            }
        }

        public bool PartnerEditable
        {
            get
            {
                return _partnerEditable;
            }
            set
            {
                _partnerEditable = value;
                RaisePropertyChanged("PartnerEditable");
            }
        }

        public ICommand MoveToNextCommand {
            get 
            {
                return new RelayCommand(
                    MoveToNextPartner, 
                    () => _next != null
                ); 
            }
        }

        public ICommand SendCommand
        {
            get
            {
                return new RelayCommand(
                    SendMail, 
                    () => Partner.ContactEmail != null || Partner.Email != null);
            }
        }

        public ICommand MakePartnerEditableCommand
        {
            get
            {
                return new RelayCommand(
                    () => TogglePartnerEditable()
                );
            }
        }

        public ICommand PrintCommand()
        {
            return new RelayCommand(
                ()=>Print()
            );
        }

        void Print()
        {
            DocumentPrinter.Print(Settings.CallReportPrintTemplatePath, CallInfo);
        }

        void TogglePartnerEditable()
        {
            PartnerEditable = !PartnerEditable;
        }

        void MoveToNextPartner()
        {
            Save();
            ShowView(new CallInfoViewModel(_singleProjectView, Project, _next));
        }

        void SendMail()
        {
            ShowView(new MailInfoViewModel(this, Project, new List<Partner>{ Partner } ));
        }

        public override void Save()
        {
            using (var tx = Session.BeginTransaction())
            {
                Session.SaveOrUpdate(CallInfo);
                Session.SaveOrUpdate(Partner);
                Session.SaveOrUpdate(Project);

                tx.Commit();
            }

            Close();
        }

        public bool FirstCallEnable { get { return true; } }
        public bool SecondCallEnable { get { return String.IsNullOrEmpty(CallInfo.FirstCallee) == false; } }

        private bool _partnerEditable;
        private readonly SingleProjectViewModel _singleProjectView;
        private PartnerInfoCall _next;
    }
}
