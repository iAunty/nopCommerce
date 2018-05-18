using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace Nop.Data.Seed
{
    public abstract class NopSeed<T> : INopSeed where T : BaseEntity
    {
        protected virtual bool IsValid => true;

        /// <summary>
        /// Seed
        /// </summary>
        /// <param name="dbContext"></param>
        public void Seed(MigrationDBContext dbContext)
        {
            //block if not need to seed the data
            if(!IsValid)
                return;

            //get dbSet
            var dbSet = dbContext.Set<T>();

            //if table is null
            if (IsValid && !dbSet.Any())
            {
                //insert default data
                InsertDateIfTableIsEmpty(dbSet);
            }
        }

        /// <summary>
        /// insert default data if 
        /// </summary>
        /// <param name="dbSet"></param>
        public abstract void InsertDateIfTableIsEmpty(IDbSet<T> dbSet);
    }
}
