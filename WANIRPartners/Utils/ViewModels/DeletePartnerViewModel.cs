using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmFoundation.Wpf;
using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.ViewModels
{
    public class DeletePartnerViewModel : CommonChildPageViewModel<Partner>
    {
        public DeletePartnerViewModel(PageViewModel parent, Partner partner)
            : base(parent)
        {
            Partner = partner;
        }

        public override String ViewName
        {
            get { return Const.PARTNER_CREATE_CAPTION; }
        }

        public override ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                    new NamedCommand(Const.YES_CAPTION, new RelayCommand(Delete)),
                    new NamedCommand(Const.NO_CAPTION, new RelayCommand(Cancel))
                };
            }
        }

        public Partner Partner{ get; private set; }

        private void Delete()
        {
            using (var tx = Session.BeginTransaction())
            {
                Session.Delete(Partner);
                tx.Commit();
            }

            Close();
        }
    }
}
