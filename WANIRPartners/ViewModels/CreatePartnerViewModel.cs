using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmFoundation.Wpf;
using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    class CreatePartnerViewModel : CommonChildPageViewModel<Partner>
    {
        public CreatePartnerViewModel(PageViewModel parent)
            : base(parent)
        {}

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

        public   int Id { get; protected set; }
        public   string Phone { get; set; }
        public   string Gmina { get; set; }
        public   string ContactPerson { get; set; }
        public   string Email { get; set; }
        public   string Address { get; set; }
        public   string Position { get; set; }
        public   string Department { get; set; }
        public   string ContactAddress { get; set; }
        public   string ContactPhone { get; set; }
        public   string ContactEmail { get; set; }
        public   string Region { get; set; }
        public   string Comment { get; set; }
        public   int CooperationYear { get; set; }
        public   bool ProjectWritingAndRealization { get; set; }
        public   bool ProjectWriting { get; set; }
        public   bool ProjectMeeting { get; set; }
        public   bool ProjectRealization { get; set; }
        public   string AcquiredBy { get; set; }
        public   string ServicedBy { get; set; }
        public   string Other { get; set; }
    }
}
