using System;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Input;

using MvvmFoundation.Wpf;

using NHibernate.Criterion;
using NHibernate.Linq;

using WANIRPartners.Models;
using WANIRPartners.Utils;
using WANIRPartners.Utils.Doc;

namespace WANIRPartners.ViewModels
{
    public class PartnersViewModel : PageViewModel
    {
        public PartnersViewModel(IViewController controller)
            : base(controller)
        {
        }

        override public string ViewName
        {
            get { return Const.PARTNERS_CAPTION; }
        }
        
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(
                    () => RaisePropertyChanged("Partners")
                );
            }
        }

        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                    new NamedCommand(Const.ADD_CAPTION, new RelayCommand(AddPartnerCommand)), 
                    
                    new NamedCommand(Const.DELETE_CAPTION, 
                        new RelayCommand(DeletePartnerCommand,
                        () => SelectedPartner != null)),
 
                    new NamedCommand(Const.EDIT_CAPTION, 
                        new RelayCommand(EditPartnerCommand,
                        () => SelectedPartner != null)),

                    new NamedCommand(Const.IMPORT_CAPTION, new RelayCommand(ImportPartnersCommand)), 
                    new NamedCommand(Const.EXPORT_CAPTION, new RelayCommand(ExportPartnersCommand)), 
                    
                    new NamedCommand(Const.PRINT_CAPTION, 
                        new RelayCommand(PrintPartnerCommand, 
                            () => SelectedPartner != null)), 
                };
            }
        }
        
        public Partner SelectedPartner { get; set; }

        public ObservableCollection<Partner> Partners
        {
            get
            {
                List<SimpleExpression> criteria = new List<SimpleExpression>();

                var query = Session.CreateCriteria<Partner>();

                if (!String.IsNullOrEmpty(Name))
                    criteria.Add(Restrictions.Like("Name", String.Format("%{0}%", Name)));
                
                if (!String.IsNullOrEmpty(Province) && Province != Const.NOT_SET)
                    criteria.Add(Restrictions.Eq("Province", Province));

                if (!String.IsNullOrEmpty(District) && District != Const.NOT_SET)
                    criteria.Add(Restrictions.Eq("District", District));
                
                if (!String.IsNullOrEmpty(Type))
                    criteria.Add(Restrictions.Eq("Type", Type));

                if (!String.IsNullOrEmpty(FullText))
                {
                    foreach (var col in new string[] { 
                        "Name", "Province", "District", "Email", "Other", 
                        "Address", "Position", "Department", "ContactAddress",
                        "ContactPhone", "Phone", "ContactEmail", "Region", 
                        "AcquiredBy", "ServicedBy", "Comment" })
                    {
                        criteria.Add(Restrictions.Like("Name", String.Format("%{0}%", FullText)));
                    }
                }

                if(criteria.Count() > 1)
                {
                    var or = Restrictions.Disjunction();
                    foreach(SimpleExpression exp in criteria)
                    {
                        or.Add(exp);
                    }
                    query.Add(or);
                }
                else if(criteria.Count() == 1)
                {
                    query.Add(criteria.First());
                }

                return new ObservableCollection<Partner>(query.List<Partner>().ToList());
            }
        }

        private void AddPartnerCommand()
        {
            ShowView(new CreateEditPartnerViewModel(this, null));
        }

        private void EditPartnerCommand()
        {
            ShowView(new CreateEditPartnerViewModel(this, SelectedPartner));
        }

        private void DeletePartnerCommand()
        {
            ShowView(new DeletePartnerViewModel(this, SelectedPartner));
        }

        private void ImportPartnersCommand()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files (*.xls, *.xlsx)|*.xls;*.xlsx";
            if (dialog.ShowDialog() == true)
            {
                PartnersExcelImporter.Import(dialog.FileName, Session, true);
            }
        }

        private void ExportPartnersCommand()
        {
            var dialog = new SaveFileDialog();
            if(dialog.ShowDialog() == true)
            {
                new ExcelExporter<Partner>(dialog.FileName).Export(
                    Partners.ToList<Partner>(), null, Const.PARTNERS_SCHEMA);
            }
        }

        private void PrintPartnerCommand()
        {
            DocumentPrinter.Print(Settings.PartnerPrintTemplatePath, SelectedPartner);
        }
    }
}
