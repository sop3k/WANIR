using System;

using FluentNHibernate.Mapping;
using WANIRPartners.Models;

namespace WANIRPartners.Mappings
{
    public class ProjectMap : ClassMap<Project>
    {
        public ProjectMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.District);
            Map(x => x.Province);
            Map(x => x.Type);
            Map(x => x.Mailing);

            HasMany(x => x.Calls).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Mails).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
