using FluentNHibernate.Mapping;
using WANIRPartners.Models;

namespace WANIRPartners.DB.Mappings
{
    public class CallInfoMap : ClassMap<CallInfo>
    {
        public CallInfoMap()
        {
            Id(x => x.Id);
            Map(x => x.Callee);
            Map(x => x.Info);
            References(x => x.Partner);
            References(x => x.Project);           
        }
    }
}