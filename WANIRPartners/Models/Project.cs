using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WANIRPartners.Models
{
    public class Project
    {
        public Project(String name)
        {
            Name = name;
        }

       public String Name { get; set; }
    }
}
