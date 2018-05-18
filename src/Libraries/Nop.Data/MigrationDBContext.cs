using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data.Migrations;

namespace Nop.Data
{
    /// <summary>
    /// This class is used for auto-migration
    /// And avoid multi-ctors in <see cref="NopObjectContext"/>
    /// </summary>
    public class MigrationDBContext : NopObjectContext
    {
        private const string ConnectionStringName = "Name=MS_TableConnectionString";

        /// <summary>
        /// Ctor
        /// this Ctor should called by <see cref="Configuration"/> when called update-datbase
        /// </summary>
        public MigrationDBContext() : base(ConnectionStringName)
        {
            this.Database.CreateIfNotExists();
        }
    }
}
