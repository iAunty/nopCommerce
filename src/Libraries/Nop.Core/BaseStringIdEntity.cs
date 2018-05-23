using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Mobile.Server.Tables;

namespace Nop.Core
{
    /// <summary>
    /// Base Guid entity 
    /// is for object that id is GUID
    /// </summary>
    public abstract class BaseStringIdEntity : IBaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        [Key]
        [TableColumn(TableColumnType.Id)]
        public string Id { get; set; }

        /// <summary>
        /// Is Equal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as BaseStringIdEntity);
        }

        /// <summary>
        /// Is Transient
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool IsTransient(BaseStringIdEntity obj)
        {
            return obj != null && Equals(obj.Id, default(string));
        }

        /// <summary>
        /// Get type 
        /// </summary>
        /// <returns></returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }

        /// <summary>
        /// is equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool Equals(BaseStringIdEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                       otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        /// <summary>
        /// Get hesh code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        /// <summary>
        /// operator
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator ==(BaseStringIdEntity x, BaseStringIdEntity y)
        {
            return Equals(x, y);
        }

        /// <summary>
        /// operator
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool operator !=(BaseStringIdEntity x, BaseStringIdEntity y)
        {
            return !(x == y);
        }
    }
}
