using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if(propertyChangedEventArgs.PropertyName != "Partners")
                RaisePropertyChanged("Partners");
        }

        override public string ViewName
        {
            get { return Const.PARTNERS_CAPTION; }
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
                var query = Session.CreateCriteria<Partner>();
                
                if (!String.IsNullOrEmpty(Name))
                    query.Add(Restrictions.Eq("Name", Name));
                if (!String.IsNullOrEmpty(Province))
                    query.Add(Restrictions.Eq("Province", Province));
                if (!String.IsNullOrEmpty(District))
                    query.Add(Restrictions.Eq("District", District));
                if (!String.IsNullOrEmpty(Type))
                    query.Add(Restrictions.Eq("Type", Type));
                
                return new ObservableCollection<Partner>(query.List<Partner>().ToList());
            }
        }
        private void AddPartnerCommand()
        {
            ShowView(new CreatePartnerViewModel(this));
        }

        private void DeletePartnerCommand()
        {
            ShowView(new DeletePartnerViewModel(this, SelectedPartner));
        }

        private void ImportPartnersCommand()
        { }
        private void ExportPartnersCommand()
        { }

        private void PrintPartnerCommand()
        {
            DocumentPrinter.Print(Settings.PartnerPrintTemplatePath, SelectedPartner);
        }
    }
}
