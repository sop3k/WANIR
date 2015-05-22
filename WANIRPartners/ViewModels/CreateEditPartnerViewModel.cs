using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmFoundation.Wpf;
using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    class CreateEditPartnerViewModel : CommonChildPageViewModel<Partner>
    {
        public CreateEditPartnerViewModel(PageViewModel parent, Partner partner)
            : base(parent)
        {
            editedPartner = partner;
            if (editedPartner != null)
                Fill(editedPartner);
        }

        override public String ViewName
        {
            get { return Const.PARTNER_CREATE_CAPTION; }
        }

        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                    new NamedCommand(Const.SAVE_CAPTION, new RelayCommand(Save)),
                    new NamedCommand(Const.CANCEL_CAPTION, new RelayCommand(Cancel))
                };
            }
        }

        private void Fill(Partner partner)
        {
            var populator = new ObjectPopulator<CreateEditPartnerViewModel>(this);
            populator.Populate(partner);
        }

        public virtual void Save()
        {
            using (var tx = Session.BeginTransaction())
            {
                Partner obj = editedPartner ?? new Partner();
                obj.Populate(this);
                Session.SaveOrUpdate(obj);
                tx.Commit();
            }

            Close();
        }

        public  int Id { get; protected set; }
        public  string Phone { get; set; }
        public  string Gmina { get; set; }
        public  string ContactPerson { get; set; }
        public  string Email { get; set; }
        public  string Address { get; set; }
        public  string Position { get; set; }
        public  string Department { get; set; }
        public  string ContactAddress { get; set; }
        public  string ContactPhone { get; set; }
        public  string ContactEmail { get; set; }
        public  string Comment { get; set; }
        public  int CooperationYear { get; set; }
        public  bool ProjectWritingAndRealization { get; set; }
        public  bool ProjectWriting { get; set; }
        public  bool ProjectMeeting { get; set; }
        public  bool ProjectRealization { get; set; }
        public  string AcquiredBy { get; set; }
        public  string ServicedBy { get; set; }
        public  string Other { get; set; }
        public  int SP { get; set; }
        public  int G { get; set; }
        public  int LO { get; set; }
        public  int ZSZTECH { get; set; }
        public  int P { get; set; }

        public  string Projects { get; set; }
        public  string Aggrements { get; set; }


        private Partner editedPartner;
    }
}
