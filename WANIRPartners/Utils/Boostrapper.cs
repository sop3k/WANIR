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
//#if DEBUG
//            SessionFactory = DB.SQLiteSessionFactory.CreateSessionFactory();
//#else
            SessionFactory = DB.PGSQLSessionFactory.CreateSessionFactory();
//#endif
            Provinces = ProvincesFactory.Initialize();
#if False
            using (var session = SessionFactory.OpenSession())
            {
                Models.PartnerGenerator.GeneratePartners(session, 100, "");
            }
#endif
        }
    }
}
