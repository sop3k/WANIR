using System;
using System.Collections.Generic;

using NHibernate;

namespace WANIRPartners.Models
{
    public class Project : ObjectPopulator<Project>
    {
        public Project() { }

        virtual public IEnumerable<Partner> Partners { get  { return new List<Partner>(); } }

        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual string Province { get; set; }
        public virtual string District { get; set; }
        public virtual bool Mailing { get; set; }
        public virtual string Type { get; set; }

        public virtual IList<CallInfo> Calls { get; set; }
        public virtual IList<MailInfo> Mails { get; set; }
    }

    class ProjectGenerator
    {
        public static void GenerateProjects(ISession session, int count, string prefix)
        {
            using (var tx = session.BeginTransaction())
            {

                for (int i = 0; i < count; i++)
                {
                    var rand = new Random();
                    var p = new Project {Name = String.Format("{0}_PROJECT_{1}", prefix, i)};
                    session.Save(p);
                }
                tx.Commit();
            }
        }
    }
}
