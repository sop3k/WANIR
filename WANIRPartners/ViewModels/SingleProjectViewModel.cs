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
    public class PartnerInfoCall
    {
        public PartnerInfoCall(Partner partner, CallInfo info)
        {
            Id = partner.Id;
            Partner = partner;
            CallInfo = info;
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

        override public string ViewName
        {
            get { return CurrentProject.Name; }
        }
       
        public PartnerInfoCall CurrentItem { get; set; }
        
        public Project CurrentProject{ get; set; }
        
        public Partner CurrentPartner
        {
            get { return CurrentItem.Partner; }
        }
        
        public CallInfo CurrentCallInfo
        {
            get { return CurrentItem.CallInfo; }
        }
        
        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                    new NamedCommand(Const.EDIT_CAPTION, 
                        new RelayCommand( 
                            () => ShowView(new CallInfoViewModel(this, CurrentProject, CurrentItem)),
                            () => CurrentItem != null && !CurrentProject.Mailing)),

                    new NamedCommand(Const.PRINT_CAPTION, 
                        new RelayCommand(
                            PrintCallInfo, 
                            () => CurrentItem != null && CurrentItem.CallInfo != null))
                };
            }
        }
        
        public IEnumerable<PartnerInfoCall> PartnersWithCallInfo
        {
            get
            {
                return from partner in Session.Query<Partner>()
                    where partner.Province == CurrentProject.Province
                          && partner.District == CurrentProject.District
                          && partner.Type == CurrentProject.Type
                    from call in partner.Calls.DefaultIfEmpty()
                    select new PartnerInfoCall(partner, call);
            }
        }
        
        private void PrintCallInfo()
        {
            DocumentPrinter.Print(Settings.CallReportPrintTemplatePath, CurrentCallInfo);
        }
    }
}