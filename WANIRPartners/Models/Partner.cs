using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Util;
using WANIRPartners.Models;
using WANIRPartners.Utils;

namespace WANIRPartners.Models
{
    public class Partner : ObjectPopulator<Partner>
    {
        public Partner() {}
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Type { get; set; }
        public virtual string Gmina { get; set; }
        public virtual string Province { get; set; }
        public virtual string District { get; set; }
        public virtual string ContactPerson { get; set; }
        public virtual string Email { get; set; }
        public virtual string Address{ get; set; }
        public virtual string Position { get; set; }
        public virtual string Department { get; set; }
        public virtual string ContactAddress { get; set; }
        public virtual string ContactPhone { get; set; }
        public virtual string ContactEmail { get; set; }
        public virtual string Region { get; set; }
        public virtual string Comment { get; set; }

        public virtual bool Cooperation { get; set; }
        public virtual bool ProjectWritingAndRealization { get; set; }
        public virtual bool ProjectWriting { get; set; }
        public virtual bool ProjectMeeting { get; set; }
        public virtual bool ProjectRealization { get; set; }
        public virtual bool ProjectWritingInAssessment { get; set; }
        public virtual bool ProjectRealizationOther { get; set; }

        public virtual string CooperationStr { get { return Const.Convert(Cooperation); } }
        public virtual string ProjectWritingAndRealizationStr { get { return Const.Convert(ProjectWritingAndRealization);  } }
        public virtual string ProjectWritingStr { get { return Const.Convert(ProjectWriting); } }
        public virtual string ProjectMeetingStr { get { return Const.Convert(ProjectMeeting); } }
        public virtual string ProjectRealizationStr { get { return Const.Convert(ProjectRealization); } }
        public virtual string ProjectWritingInAssessmentStr { get { return Const.Convert(ProjectWritingInAssessment); } }
        public virtual string ProjectRealizationOtherStr { get { return Const.Convert(ProjectRealizationOther); } }

        public virtual string ProjectWritingAndRealizationDetails { get; set; }
        public virtual string ProjectWritingDetails { get; set; }
        public virtual string ProjectMeetingDetails { get; set; }
        public virtual string ProjectRealizationDetails { get; set; }
        virtual public string ProjectWritingInAssessmentDetails { get; set; }
        virtual public string ProjectRealizationOtherDetails { get; set; }
        public virtual string AcquiredBy { get; set; }
        public virtual string ServicedBy { get; set; }
        public virtual string Other { get; set; }
        public virtual int SP { get; set; }
        public virtual int G { get; set; }
        public virtual int LO { get; set; }
        public virtual int ZSZTECH { get; set; }
        public virtual int P { get; set; }

        public virtual string Projects { get; set; }
        public virtual string Aggrements { get; set; }

        public virtual IList<CallInfo> Calls{ get; set; }
        public virtual IList<MailInfo> Mails { get; set; }
        public virtual IList<Project> RemovedFrom { get; set; }
    }

    class PartnerGenerator
    {
        public static void GeneratePartners(ISession session, int count, string prefix)
        {
            using (var tx = session.BeginTransaction())
            {
                var r = new Random();
                for (int i = 0; i < count; i++)
                {
                    var p = new Partner
                    {
                        Name = String.Format("{0}_{1}", prefix, i),
                        Email = String.Format("{0}_{1}@com.pl", prefix, i),
                        Phone = String.Format("{0}", r.Next(1000000)),
                        Type = Const.Types[r.Next(0, Const.Types.Count)],
                        Province = Const.Provinces.Keys.First(),
                        District = Const.Provinces[Const.Provinces.Keys.First()].First()
                        
                    };
                    session.Save(p);
                }

                tx.Commit();
            }
        }
    }
}
