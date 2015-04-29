using System;
using System.Collections.Generic;

namespace WANIRPartners.Models
{
    public class Project
    {
        public Project(string name)
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

        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
    }
}
