using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Data.Mapping;

namespace iAunty.com.Data.Mapping
{
    public class NopGuidEntityTypeConfiguration<T> : NopEntityTypeConfiguration<T> where T : BaseStringIdEntity
    {
        protected NopGuidEntityTypeConfiguration()
        {
            
        }

        protected override void PostInitialize()
        {
            base.PostInitialize();
            //this.HasKey(x => x.Id);
        }
    }
}
