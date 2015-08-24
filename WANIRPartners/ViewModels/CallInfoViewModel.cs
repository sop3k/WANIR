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
            
            //To set CallInfo as soon as possible.
            //Save(false);
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

        public DateTime OfferDate
        {
            get
            {
                if (Offer)
                    return CallInfo.OfferDate.Value;
                else
                    return DateTime.Now;
            }

            set
            {
                CallInfo.OfferDate = value;
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
        public bool OfferStr
        {
            get { return CallInfo.Offer; }
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
                    MoveToNextPartner
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
            Save(false);
        }

        void MoveToNextPartner()
        { 

            var items = _singleProjectView.Items.ToList();
            var currentIndex = items.FindIndex(p => p.Partner == Partner);

            for(int i=currentIndex+1; i<_singleProjectView.Items.Count(); i++)
            {
                var next = Session.QueryOver<CallInfo>()
                    .Where(p => p.Partner == items[i].Partner && p.Project == items[i].Project)
                    .SingleOrDefault();

                if (next== null)
                {
                    Save(true);
                    ShowView(new CallInfoViewModel(
                        _singleProjectView, 
                        Project, 
                        new PartnerInfoCall(Project, items[i].Partner, null)));
                    
                    break;
                }       
            }
        }

        void SendMail()
        {
            ShowView(new MailInfoViewModel(this, Project, new List<Partner>{ Partner } ));
        }

        public override void Save()
        {
            Save(true);
        }

        public void Save(bool close)
        {
            using (var tx = Session.BeginTransaction())
            {
                Session.SaveOrUpdate(CallInfo);
                Session.SaveOrUpdate(Partner);
                Session.SaveOrUpdate(Project);

                tx.Commit();
            }

            if(close)
                Close();
        }

        private bool _partnerEditable;
        private readonly SingleProjectViewModel _singleProjectView;
        private PartnerInfoCall _next;
    }
}
