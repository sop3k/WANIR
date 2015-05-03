using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace WANIRPartners.Utils
{
    class Boostrapper
    {
        public static ISessionFactory SessionFactory { get; private set; }
        public static Dictionary<String, List<String>> Provinces { get; private set; } 

        public static void Initialize()
        {
            SessionFactory = DB.SQLiteSessionFactory.CreateSessionFactory();
            Provinces = ProvincesFactory.Initialize();
#if DEBUG
            using (var session = SessionFactory.OpenSession())
            {
                Models.PartnerGenerator.GeneratePartners(session, 100, "");
            }
#endif
        }
    }
}
