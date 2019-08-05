using JIYITECH.WebApi.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Factories
{
    public static class SqlServerTableFactory
    {
        //static string connectionString = "server=172.17.13.13;database=smartcoal;user=yqrl;pwd=abcd@1234;";
        static public string connectionString { get; set; }

        public static bool GenerateTable<T>()
        {
            var dbFactory = new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider);
            using (var db = dbFactory.Open())
            {
                if (db.CreateTableIfNotExists<T>())
                {
                    return true;
                }
                else
                {
                    foreach (PropertyInfo prop in typeof(T).GetProperties())
                    {
                        AliasAttribute attribute = (AliasAttribute)typeof(T).GetCustomAttributes(typeof(AliasAttribute), false).FirstOrDefault();
                        if (!db.ColumnExists(prop.Name, attribute.Name)) //= false
                        {
                            var modelDef = ModelDefinition<T>.Definition;
                            if (modelDef.IgnoredFieldDefinitions.FindAll(x => x.FieldName == prop.Name).Count == 0)
                            {
                                var fieldDef = modelDef.GetFieldDefinition(prop.Name);
                                db.AddColumn(typeof(T), fieldDef);
                            }
                        }
                    }
                }
                return false;
            }
        }
    }
}
