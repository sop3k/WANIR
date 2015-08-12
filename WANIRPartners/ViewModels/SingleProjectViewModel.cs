using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;

using LinqKit;

using MvvmFoundation.Wpf;
using NHibernate.Linq;

using WANIRPartners.Models;
using WANIRPartners.Views;
using WANIRPartners.Utils;
using WANIRPartners.Utils.Doc;

namespace WANIRPartners.ViewModels
{
    public class PartnerInfoCall
    {
        public PartnerInfoCall(Project project, Partner partner, CallInfo info)
        {
            CallInfo = null;
            if(info != null && project == info.Project)
                CallInfo = info;

            Id = partner.Id;
            Partner = partner;
        }
        public int Id { get; set; }
        public string Name { get { return Partner.Name; } }
        public string District { get { return Partner.District; } }


        public Partner Partner { get; set; }
        public CallInfo CallInfo { get ; set; }
    };

    public class SingleProjectViewModel : ChildPageViewModel
    {
        public SingleProjectViewModel(PageViewModel parent, Project project)
            : base(parent)
        {
            CurrentProject = project;
        }

        public SingleProjectView View { get; set; }

        override public string ViewName
        {
            get { return CurrentProject.Name; }
        }
       
        public PartnerInfoCall CurrentItem { get; set; }
        
        public Project CurrentProject{ get; set; }
        
        public Partner CurrentPartner
        {
            get { return CurrentItem != null ? CurrentItem.Partner : null; }
        }
        
        public CallInfo CurrentCallInfo
        {
            get { return CurrentItem != null ? CurrentItem.CallInfo : null; }
        }
        
        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                    new NamedCommand(Const.ADD_PARTNER_CAPTION, 
                        new RelayCommand(AddPartner)),
                    
                    new NamedCommand(Const.SEND_CAPTION, 
                        new RelayCommand(SendMail, 
                            () => CurrentPartner != null)),

                    new NamedCommand(Const.REMOVE_PARTNER_FROM_PROJECT, 
                        new RelayCommand(RemovePartnerFromProject,
                            () => CurrentPartner != null)),

                    new NamedCommand(Const.PRINT_CAPTION, 
                        new RelayCommand(
                            PrintCallInfo, 
                            () => CurrentItem != null && CurrentItem.CallInfo != null)),

                    new NamedCommand(Const.EXPORT_PROJECT_CAPTION, 
                        new RelayCommand(ExportProjectCommand)),
                };
            }
        }

        override public ObservableCollection<NamedCommand> SpecificCommands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                     new NamedCommand(Const.CALL_CAPTION, 
                            new RelayCommand( 
                                () => ShowView(new CallInfoViewModel(this, CurrentProject, CurrentItem)),
                                () => CurrentItem != null && !CurrentProject.Mailing))
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

            if (CurrentProject.RemovedPartners != null)
            {
                predicate = predicate.And(p => !CurrentProject.RemovedPartners.Contains(p));
            }
            return predicate;
        }

        public IEnumerable<PartnerInfoCall> PartnersWithCallInfo
        {
            get
            {
                return from partner in Session.Query<Partner>()
                           .Where(PartnersWhereClause())
                       from call in partner.Calls.DefaultIfEmpty()
                    select new PartnerInfoCall(CurrentProject, partner, call);
            }
        }

        public IEnumerable<PartnerInfoCall> Items
        {
            get { return View.PartnersGrid.Items.Cast<PartnerInfoCall>(); }
        }
        
        private void PrintCallInfo()
        {
            DocumentPrinter.Print(Settings.CallReportPrintTemplatePath, CurrentCallInfo);
        }

        private void ExportProjectCommand()
        {
            var dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                new ExcelExporter<PartnerInfoCall>(dialog.FileName).Export(
                    PartnersWithCallInfo.ToList<PartnerInfoCall>(), null, Const.PROJECTS_SCHEMA
                );
            }
        }

        private void AddPartner()
        {
            ShowView(new CreateEditPartnerViewModel(this.Parent, null));
        }

        private void RemovePartnerFromProject()
        {
            using (var tx = Session.BeginTransaction())
            {
                CurrentProject.RemovedPartners.Add(CurrentPartner);
                CurrentPartner.RemovedFrom.Add(CurrentProject);

                Session.SaveOrUpdate(CurrentProject);
                Session.SaveOrUpdate(CurrentPartner);
                
                tx.Commit();
            }
        }
       
        private void SendMail()
        {
            ShowView(new MailInfoViewModel(this.Parent, CurrentProject, new List<Partner> { CurrentPartner }));
        }
    }
}