using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MvvmFoundation.Wpf;
using WANIRPartners.Utils;
using WANIRPartners.Models;
using Outlook = Microsoft.Office.Interop.Outlook; 

namespace WANIRPartners.ViewModels
{
    public class MailInfoViewModel : CommonChildPageViewModel<MailInfo>
    {
        public MailInfoViewModel(ChildPageViewModel parent, Project project, IEnumerable<Partner> partners)
            : base(parent.Parent)
        {
            Project = project;
            Partners = partners;
            Date = DateTime.Now;
        }

        #region View
        override public String ViewName
        {
            get { return "NOWY MAIL"; }
        }

        override public ObservableCollection<NamedCommand> Commands
        {
            get
            {
                return new ObservableCollection<NamedCommand>
                {
                    new NamedCommand(Const.SAVE_CAPTION, new RelayCommand(Save)),
                    new NamedCommand(Const.CANCEL_CAPTION, new RelayCommand(Close)),
                    new NamedCommand(Const.OUTLOOK_OPEN, new RelayCommand(
                        () => OpenOutlook(Partners, Subject))),
                };
            }
        }
        #endregion

        public IEnumerable<Partner> Partners { get; private set; }

        public Project Project { get; private set; }

        public DateTime Date { get; set; }

        public string Subject { get; set; }

        public override void Save()
        {
            using (var tx = Session.BeginTransaction())
            {
                foreach (var partner in Partners)
                {
                    MailInfo info = new MailInfo();

                    info.Partner = partner;
                    info.Date = Date;
                    info.Subject = Subject;
                    info.Project = Project;

                    Session.Save(info);
                }

                tx.Commit();
            }

            Close();
        }
    
        private void OpenOutlook(IEnumerable<Partner> partners, string subject)
        {
            try
            {
                Outlook.Application outlookApp = new Outlook.Application();
                Outlook._MailItem oMailItem = (Outlook._MailItem)outlookApp.CreateItem(Outlook.OlItemType.olMailItem);
                Outlook.Inspector oInspector = oMailItem.GetInspector;

                // Recipient
                Outlook.Recipients oRecips = (Outlook.Recipients)oMailItem.Recipients;
                foreach (Partner recipient in partners)
                {
                    string email = recipient.ContactEmail ?? recipient.Email;

                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(email);
                    oRecip.Type = (int)Outlook.OlMailRecipientType.olTo;
                    oRecip.Resolve();

                }

                oMailItem.Subject = subject;
                oMailItem.Display(true);
            }
            catch (Exception objEx)
            {
                MessageBox.Show(objEx.ToString());
            }
        }
    }
}
