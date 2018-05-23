using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Microsoft.Azure.Mobile.Server.Tables;

namespace Nop.Core.Data
{
    /// <summary>
    /// Syncable Repository
    /// </summary>
    public interface ISyncableRepository<TData> : IDomainManager<TData> where TData : class , ITableData , IBaseEntity
    {
        #region Property

        /// <summary>
        /// HttpRequestMessage
        /// </summary>
        HttpRequestMessage Request { get; set; }

        #endregion


        #region Function

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TData GetById(string id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TData GetById(Guid id);

        /// <summary>
        /// Executes the provided <paramref name="query"/> against a store.
        /// </summary>
        /// <remarks>
        /// See also <see cref="M:LookupAsync"/> which is the companion method for executing a lookup for a single item.
        /// </remarks>
        /// <param name="query">The <see cref="ODataQueryOptions"/> query to execute.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> representing the result of the query.</returns>
        IEnumerable<TData> Query(ODataQueryOptions query);

        /// <summary>
        /// Inserts an item to the backend store.
        /// </summary>
        /// <param name="data">The data to be inserted</param>
        /// <returns>The inserted item.</returns>
        TData Insert(TData data);

        /// <summary>
        /// Updates an existing item by applying a <see cref="Delta{T}"/> patch to it. The <see cref="Delta{T}"/>
        /// abstraction keeps track of which properties have changed which avoids problems with default values and
        /// the like.
        /// </summary>
        /// <param name="id">The id of the item to patch.</param>
        /// <param name="patch">The patch to apply.</param>
        /// <returns>The patched item.</returns>
        TData Update(string id, Delta<TData> patch);

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        TData Update(TData data);

        /// <summary>
        /// Completely replaces an existing item.
        /// </summary>
        /// <param name="id">The id of the item to replace.</param>
        /// <param name="data">The replacement</param>
        /// <returns>The replaced item</returns>
        TData Replace(string id, TData data);

        /// <summary>
        /// Deletes an existing item
        /// </summary>
        /// <param name="id">The id of the item to delete.</param>
        /// <returns><c>true</c> if item was deleted; otherwise <c>false</c></returns>
        bool Delete(string id);

        /// <summary>
        /// Deletes an existing item
        /// </summary>
        /// <param name="data">The id of the item to delete.</param>
        /// <returns><c>true</c> if item was deleted; otherwise <c>false</c></returns>
        bool Delete(TData data);

        #endregion


        #region AsyncableFunction

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<TData> UpdateAsync(TData data);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TData> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<TData> TableNoTracking { get; }

        #endregion
    }
}
