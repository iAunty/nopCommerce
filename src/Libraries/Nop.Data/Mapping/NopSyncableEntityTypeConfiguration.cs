using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace iAunty.com.Data.Mapping
{
    public class NopSyncableEntityTypeConfiguration<T> : NopGuidEntityTypeConfiguration<T> where T : BaseSyncableEntity
    {
        protected NopSyncableEntityTypeConfiguration()
        {

        }

        protected override void PostInitialize()
        {
            base.PostInitialize();
            //this.HasRequired(x => x.CreatedAt.Value);//TODO : not null
            //this.HasRequired(x => x.Version);
        }
    }
}
