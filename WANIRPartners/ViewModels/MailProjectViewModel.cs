using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using MvvmFoundation.Wpf;

using NHibernate.Linq;

using WANIRPartners.Models;
using WANIRPartners.Utils;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using WANIRPartners.Utils.Doc;

namespace WANIRPartners.ViewModels
{
    public class MailProjectViewModel : ChildPageViewModel
    {
        public MailProjectViewModel(PageViewModel parent, Project project)
            : base(parent)
        {
            CurrentProject = project;
        }

        override public string ViewName
        {
            get { return CurrentProject.Name; }
        }

        public Project CurrentProject { get; set; }

        public Partner CurrentPartner
        {
            get
            {
                return _currentPartner;
            }

            set
            {
                _currentPartner = value;
                RaisePropertyChanged("CurrentPartner");
            }
        }

        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                     new NamedCommand(Const.SEND_ALL_CAPTION, 
                        new RelayCommand(() => SendMail(Partners))),
                
                    new NamedCommand(Const.SEND_CAPTION, 
                        new RelayCommand(() => SendMail(CurrentPartner)))
                };
            }
        }

        public IEnumerable<Partner> Partners
        {
            get
            {
                return from partner in Session.Query<Partner>()
                       where partner.Province == CurrentProject.Province
                             && partner.District == CurrentProject.District
                             && partner.Type == CurrentProject.Type
                       select partner;
            }
        }

        public IEnumerable<MailInfo> CurrentPartnerMails
        {
            get { return CurrentPartner.Mails.Where(x => x.Project == CurrentProject); }
        }

        private void SendMail(Partner partner)
        {
            ShowView(new MailInfoViewModel(this, CurrentProject, new List<Partner> { partner }));
        }

        private void SendMail(IEnumerable<Partner> partners)
        {
            ShowView(new MailInfoViewModel(this, CurrentProject, partners));
        }

        private Partner _currentPartner;
    }
}