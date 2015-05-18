using FluentNHibernate.Mapping;
using WANIRPartners.Models;

namespace WANIRPartners.DB.Mappings
{
    public class CallInfoMap : ClassMap<CallInfo>
    {
        public CallInfoMap()
        {
            Id(x => x.Id);

            Map(x => x.FirstCallee);
            Map(x => x.FirstInfo);
            Map(x => x.FirstDate);

            Map(x => x.SecondCallee);
            Map(x => x.SecondInfo);
            Map(x => x.SecondDate);
            Map(x => x.Undecided);
            Map(x => x.ResignationReason);
            Map(x => x.Meeting);
            Map(x => x.MeetingPerson);
            Map(x => x.MeetingDate);
            Map(x => x.ActiveProject);
            Map(x => x.ProjectWriter);
            Map(x => x.MeetingInfo);

            References(x => x.Partner);
            References(x => x.Project);         
        }
    }
}