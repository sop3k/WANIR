using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using MvvmFoundation.Wpf;

using LinqKit;
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

        Expression<Func<Partner, bool>> PartnersWhereClause()
        {
            var predicate = PredicateBuilder.True<Partner>();

            if (!string.IsNullOrEmpty(CurrentProject.Province) && CurrentProject.Province != Const.NOT_SET)
            {
                predicate = predicate.And(p => p.Province == CurrentProject.Province);
            }

            if (!string.IsNullOrEmpty(CurrentProject.District) && CurrentProject.District != Const.NOT_SET)
            {
                predicate = predicate.And(p => p.District == CurrentProject.District);
            }

            if (!string.IsNullOrEmpty(CurrentProject.Type) && CurrentProject.Type != Const.NOT_SET)
            {
                predicate = predicate.And(p => p.Type == CurrentProject.Type);
            }

            if (!string.IsNullOrEmpty(CurrentProject.Region) && CurrentProject.Region != Const.NOT_SET)
            {
                predicate = predicate.And(p => p.Region == CurrentProject.Region);
            }

            if (CurrentProject.Cooperation.HasValue)
            {
                predicate = predicate.And(p => p.Cooperation == CurrentProject.Cooperation.Value);
            }

            return predicate;
        }

        public IEnumerable<Partner> Partners
        {
            get
            {
                return from partner in Session.Query<Partner>().Where(PartnersWhereClause())
                       select partner;
            }
        }


        public IEnumerable<MailInfo> CurrentPartnerMails
        {
            get { return CurrentPartner.Mails.Where(x => x.Project == CurrentProject); }
        }

        private void SendMail(Partner partner)
        {
            ShowView(new MailInfoViewModel(this.Parent, CurrentProject, new List<Partner> { partner }));
        }

        private void SendMail(IEnumerable<Partner> partners)
        {
            ShowView(new MailInfoViewModel(this.Parent, CurrentProject, partners));
        }

        private Partner _currentPartner;
    }
}