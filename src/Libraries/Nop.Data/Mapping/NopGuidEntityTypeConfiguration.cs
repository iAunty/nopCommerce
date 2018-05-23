using Nop.Core;

namespace Nop.Data.Mapping
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
