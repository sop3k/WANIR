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
            Project = project;
        }
        public int Id { get; set; }
        public string Name { get { return Partner.Name; } }
        public string District { get { return Partner.District; } }


        public string PartnerName{get{return Partner.Name;}}
        public string PartnerProvince{get{return Partner.Province;}}
        public string PartnerDistrict{get{return Partner.District;}}
        public string PartnerGmina{get{return Partner.Gmina;}}

        public string CallInfoFirstCallee{get{return CallInfo.FirstCallee;}}
        public string CallInfoFirstDateStr{get{return CallInfo.FirstDateStr;}}
        public string CallInfoOfferStr{get{return CallInfo.OfferStr;}}
        public string CallInfoFirstInfo{get{return CallInfo.FirstInfo;}}

        public string CallInfoSecondDateStr{get{return CallInfo.SecondDateStr;}}
        public string CallInfoMeetingStr { get { return CallInfo.MeetingStr; } }
        public string CallInfoUndecided { get { return CallInfo.Undecided; } }
        public string CallInfoResignationReason { get { return CallInfo.ResignationReason; } }

        public Partner Partner { get; set; }
        public CallInfo CallInfo { get ; set; }
        public Project Project { get; set; }
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
                            new RelayCommand( ShowCallInfoViewCall,
                                () => CurrentItem != null && !CurrentProject.Mailing))
                };
            }
        }

        void ShowCallInfoViewCall()
        {
            ShowView(new CallInfoViewModel(this, CurrentProject, CurrentItem));
        }

        Expression<Func<Partner, bool>> PartnersWhereClause()
        {
            var predicate = PredicateBuilder.True<Partner>();

            if (!string.IsNullOrEmpty(CurrentProject.Province) && CurrentProject.Province != Const.NOT_SET)
            {
                predicate = predicate.And(p => CurrentProject.Province.Split(',').Contains(p.Province));
            }

            if (!string.IsNullOrEmpty(CurrentProject.District) && CurrentProject.District != Const.NOT_SET)
            {
                predicate = predicate.And(p => CurrentProject.District.Split(',').Contains(p.District));
            }

            if (!string.IsNullOrEmpty(CurrentProject.Type) && CurrentProject.Type != Const.NOT_SET)
            {
                predicate = predicate.And(p => CurrentProject.Type.Split(',').Contains(p.Type));
            }

            if (!string.IsNullOrEmpty(CurrentProject.Region) && CurrentProject.Region != Const.NOT_SET)
            {
                predicate = predicate.And(p => CurrentProject.Region.Split(',').Contains(p.Region));
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
                var partners = from partner in Session.Query<Partner>().Where(PartnersWhereClause()) select partner;
                foreach(Partner p in partners)
                {
                    var call = p.Calls.Where(x => x.Project == CurrentProject).FirstOrDefault();
                    yield return new PartnerInfoCall(CurrentProject, p, call);
                }
            }
        }

        public IEnumerable<PartnerInfoCall> Items
        {
            get
            {
                try
                {
                    return View.PartnersGrid.Items.Cast<PartnerInfoCall>();
                }
                catch
                {
                    return new List<PartnerInfoCall>();
                }
            }
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