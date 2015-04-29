using System;
using System.Collections.Generic;

namespace WANIRPartners.Models
{
    public class Project
    {
        public Project(String name)
        {
            Name = name;
        }

        public IEnumerable<Partner> Partners
        {
            get 
            {
                return ParterGenerator.GeneratePartners(10, Name);
            }
        }

        public String Name { get; set; }
    }
}
