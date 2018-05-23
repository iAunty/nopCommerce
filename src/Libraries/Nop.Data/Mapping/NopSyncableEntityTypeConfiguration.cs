using Nop.Core;

namespace Nop.Data.Mapping
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
