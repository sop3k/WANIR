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
            Map(x => x.Cooperation).Nullable();

            HasMany(x => x.Calls).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Mails).Inverse().Cascade.AllDeleteOrphan();

            HasManyToMany(x => x.RemovedPartners)
                .Table("RemovedFromProject")
                .ParentKeyColumn("ProjectId")
                .ChildKeyColumn("PartnerId")
                .LazyLoad()
                .Cascade.SaveUpdate();
        }
    }
}
