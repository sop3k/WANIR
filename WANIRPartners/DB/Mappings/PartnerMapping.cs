using System;

using FluentNHibernate.Mapping;
using WANIRPartners.Models;

namespace WANIRPartners.Mappings
{
    public class PartnerMap : ClassMap<Partner>
    {
        public PartnerMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.LastName);
            Map(x => x.Phone);
            Map(x => x.Type);
        }
    }
}
