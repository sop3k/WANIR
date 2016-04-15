using System;
using System.IO;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace WANIRPartners.DB
{
    public class SQLiteSessionFactory
    {
        private static readonly string DbFile = System.IO.Path.Combine(System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.ApplicationData),
            "wanir.db");

        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile(DbFile))
                .Mappings(m =>m.FluentMappings.AddFromAssemblyOf<App>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
#if DEBUG
            if(System.IO.File.Exists(DbFile))
                System.IO.File.Delete(DbFile);
            new SchemaExport(config).Create(false, true);
#else
            if(!System.IO.File.Exists(DbFile))
                new SchemaExport(config).Create(false, true);
#endif

        }
    }

    public class PGSQLSessionFactory
    {
        public static ISessionFactory CreateSessionFactory()
        {
            string ip = (new StreamReader(new FileStream("database.config", FileMode.Open))).ReadLine();
            string connStr = String.Format("Server={0};Port=5432;Database=wanir_partnerzy_test;User Id=specuser;Password=5p3cu53R1;", ip);

            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard.ConnectionString(connStr))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<App>())
                .ExposeConfiguration(BuildSchema)
                .BuildSessionFactory();
        }

        private static void BuildSchema(Configuration config)
        {
#if DEBUG
            // this NHibernate tool takes a configuration (with mapping info in) 
            // new SchemaExport(config).Create(false, true);
#endif
        }
    }
}
