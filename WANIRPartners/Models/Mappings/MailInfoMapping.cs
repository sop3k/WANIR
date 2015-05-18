using System;

using FluentNHibernate.Mapping;
using WANIRPartners.Models;

namespace WANIRPartners.Models.Mappings
{
    class MailInfoMap : ClassMap<MailInfo>
    {
        public MailInfoMap()
        {
            Id(x => x.Id);
            Map(x => x.Date);
            Map(x => x.Subject);

            References(x => x.Partner);
            References(x => x.Project);
        }
    }
}
