using System;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace WANIRPartners.DB
{
    public class SessionFactory
    {
        private const string dbFile = "wanir.db";

        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(dbFile))
                .Mappings(m =>m.FluentMappings.AddFromAssemblyOf<App>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config).Create(false, true);
        }
    }
}
