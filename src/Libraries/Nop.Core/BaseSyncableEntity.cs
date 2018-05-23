using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Azure.Mobile.Server.Tables;

namespace Nop.Core
{
    /// <summary>
    /// this class is used for syncable object
    /// </summary>
    public class BaseSyncableEntity : BaseStringIdEntity , ITableData
    {
        /// <summary>
        /// Create At
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Index(IsClustered = true)]
        [TableColumn(TableColumnType.CreatedAt)]
        public DateTimeOffset? CreatedAt { get; set; }

        /// <summary>
        /// Update At
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [TableColumn(TableColumnType.UpdatedAt)]
        public DateTimeOffset? UpdatedAt { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [TableColumn(TableColumnType.Version)]
        [Timestamp]
        public byte[] Version { get; set; }

        /// <summary>
        /// Deleted
        /// </summary>
        [TableColumn(TableColumnType.Deleted)]
        public bool Deleted { get; set; }
    }
}
