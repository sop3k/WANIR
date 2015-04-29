using System;
using System.Collections.Generic;

namespace WANIRPartners.Models
{
    public class Partner
    {
        public Partner(string name, string lastname, string phone, string type)
        {
            Name = name;
            LastName = lastname;
            Phone = phone;
            Type = type;
        }

        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Phone { get; set; }
        public virtual string Type { get; set; }

    }

    class ParterGenerator
    {
        public static IEnumerable<Partner> GeneratePartners(int count, string prefix)
        {
            var r = new Random();
            String[] types = {"POWIAT", "SZKOLA", "GMINA"};
            for (int i = 0; i < count; i++)
            {
                yield return new Partner(
                    name: String.Format("{0}_{1}", prefix, i),
                    lastname: String.Format("LastName_{0}", i),
                    phone: String.Format("{0}", r.Next(1000000)),
                    type: types[r.Next(0, 3)]
                    );
            }
        }
    }
}
