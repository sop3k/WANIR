using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate;
using NHibernate.Persister.Entity;

namespace WANIRPartners.Utils
{
    class NHUtils
    {
        public static IEnumerable<string> GetPropertyColumnNames(ISessionFactory sessionFactory, object entity)
        {
            Type entityType = entity == null ? null : entity.GetType();

            List<string> columnNameList = null;

            // This has some cool methods and properties so check it out
            var metaData = entityType == null ? null : sessionFactory.GetClassMetadata(entityType.ToString());

            //- metaData validity check ... will be null if provided type is not mapped
            if (metaData != null)
            {
                // This has some even cooler methods and properties so definitely check this out
                var entityPersister = (AbstractEntityPersister) metaData;

                //- how to get the entity's identifier
                //- string entityIdentifier = metaData.IdentifierPropertyName;

                //- Get the database identifier
                //- can have multiple in case of composite keys
                IEnumerable<string> dbIdentifierNameList = entityPersister.KeyColumnNames;

                var propertyNameList = entityPersister.PropertyNames;

                // Adding the database identifier first
                columnNameList = new List<string>(dbIdentifierNameList);
                //- then add properties column names
                foreach (var propertyName in propertyNameList)
                {
                    var columnNameArray = entityPersister.GetPropertyColumnNames(propertyName);
                    columnNameList.AddRange(columnNameArray.Where(columnName => dbIdentifierNameList.Contains(columnName) == false));
                }
            }

            return columnNameList;
        }
    }
}   
