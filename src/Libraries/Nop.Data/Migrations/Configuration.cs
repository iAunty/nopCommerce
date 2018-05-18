using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nop.Data.Seed;

namespace Nop.Data.Migrations
{
    /// <summary>
    ///  use for auto-migration
    /// </summary>
    internal sealed class Configuration : DbMigrationsConfiguration<MigrationDBContext>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

            //WARNING : if un-comment might cause data-lose while auto-migration
            //AutomaticMigrationDataLossAllowed = true;

            //https://stackoverflow.com/questions/9873873/auto-create-database-tables-from-objects-entity-framework
            //set initializor
            Database.SetInitializer(new CreateDatabaseIfNotExists<MigrationDBContext>());
        }

        /// <summary>
        /// if want to insert default data in table
        /// should be added in here
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(MigrationDBContext context)
        {
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !String.IsNullOrEmpty(type.Namespace))
                .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                               type.BaseType.GetGenericTypeDefinition() == typeof(NopSeed<>));
            foreach (var type in typesToRegister)
            {
                var obj = Activator.CreateInstance(type);
                if (obj is INopSeed seedObject)
                {
                    seedObject.Seed(context);
                }
            }
            base.Seed(context);
        }
    }
}
