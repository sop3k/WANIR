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
        }
    }
}
