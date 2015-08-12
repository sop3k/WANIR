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
            Map(x => x.Phone);
            Map(x => x.Type);
            Map(x => x.Province);
            Map(x => x.Gmina);
            Map(x => x.District);
            Map(x => x.ContactPerson);
            Map(x => x.Email);
            Map(x => x.Address);
            Map(x => x.Position);
            Map(x => x.Department);
            Map(x => x.ContactAddress);
            Map(x => x.ContactPhone);
            Map(x => x.ContactEmail);
            Map(x => x.Region);
            Map(x => x.Comment);
            Map(x => x.Cooperation);
            Map(x => x.ProjectWritingAndRealization);
            Map(x => x.ProjectWriting);
            Map(x => x.ProjectMeeting);
            Map(x => x.ProjectRealization);
            Map(x => x.ProjectWritingAndRealizationDetails);
            Map(x => x.ProjectWritingDetails);
            Map(x => x.ProjectMeetingDetails);
            Map(x => x.ProjectRealizationDetails);
            Map(x => x.AcquiredBy);
            Map(x => x.ServicedBy);
            Map(x => x.Other);
            Map(x => x.SP);
            Map(x => x.G);
            Map(x => x.LO);
            Map(x => x.ZSZTECH);
            Map(x => x.P);

            HasMany(x => x.Calls).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Mails).Inverse().Cascade.AllDeleteOrphan();

            HasManyToMany(x => x.RemovedFrom)
                .Table("RemovedFromProject")
                .ParentKeyColumn("ProjectId")
                .ChildKeyColumn("PartnerId")
                .LazyLoad()
                .Cascade.SaveUpdate();
        }
    }
}
