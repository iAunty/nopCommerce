using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using Nop.Core;
using Nop.Core.Data;

namespace Nop.Data
{
    /// <summary>
    /// Entity Framework repository for syncable object
    /// TODO : need to implement 
    /// TODO : 需要實作非同步部分(create time, update time, delete, version)
    /// </summary>
    public class EfSyncableRepository<TData> : EntityDomainManager<TData> ,ISyncableRepository<TData> where TData : class, ITableData , IBaseEntity
    {
        private bool _isRequestAssign = false;
        private readonly IDbContext _dbContext;
        private IDbSet<TData> _entities;

        #region Ctor

        /// <summary>
        ///     Ctor
        /// </summary>
        public EfSyncableRepository(IDbContext context) : base(context as DbContext, new HttpRequestMessage())
        {
            _dbContext = context;

            //If need to see the sql commend,uncomment this line of code
            ((DbContext) _dbContext).Database.Log = log => System.Diagnostics.Debug.Write(log);
        }

        #endregion

        #region Field

        public new HttpRequestMessage Request
        {
            get
            {
                return base.Request;
            }

            set
            {
                _isRequestAssign = true;
                base.Request = value;
            }
        }

        public bool IsRequestAssign => _isRequestAssign;

        #endregion


        #region Function

        /// <summary>
        /// Builds an <see cref="IQueryable{T}"/> to be executed against a store supporting <see cref="IQueryable{T}"/> for querying data. 
        /// </summary>
        /// <remarks>
        /// See also <see cref="M:Lookup"/> which is the companion method for creating an <see cref="IQueryable{T}"/> representing a single item.
        /// </remarks>
        /// <returns>An <see cref="IQueryable{T}"/> which has not yet been executed.</returns>
        public override IQueryable<TData> Query()
        {
            var result = base.Query();
            return result;
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TData GetById(string id)
        {
            TData result = Lookup(id).Queryable.SingleOrDefault();
            return result;
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TData GetById(Guid id)
        {
            TData result = Lookup(id.ToString()).Queryable.SingleOrDefault();
            return result;
        }

        /// <summary>
        /// Builds an <see cref="IQueryable{T}"/> to be executed against a store supporting <see cref="IQueryable{T}"/> for looking up a single item.
        /// </summary>
        /// <param name="id">The id representing the item. The id is provided as part of the <see cref="ITableData"/> and is visible to the client. 
        /// However, depending on the backend store and the domain model, the particular implementation may map the id to some other form of unique 
        /// identifier.</param>
        /// <remarks>
        /// See also <see cref="M:Query"/> which is the companion method for creating an <see cref="IQueryable{T}"/> representing multiple items.
        /// </remarks>
        /// <returns>A <see cref="SingleResult{T}"/> containing the <see cref="IQueryable{T}"/> which has not yet been executed.</returns>
        public override SingleResult<TData> Lookup(string id)
        {
            var result = base.Lookup(id);
            return result;
        }

        /// <summary>
        /// Executes the provided <paramref name="query"/> against a store.
        /// </summary>
        /// <remarks>
        /// See also <see cref="M:LookupAsync"/> which is the companion method for executing a lookup for a single item.
        /// </remarks>
        /// <param name="query">The <see cref="ODataQueryOptions"/> query to execute.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> representing the result of the query.</returns>
        public IEnumerable<TData> Query(ODataQueryOptions query)
        {
            return QueryAsync(query).Result;
        }

        /// <summary>
        /// Inserts an item to the backend store.
        /// </summary>
        /// <param name="data">The data to be inserted</param>
        /// <returns>The inserted item.</returns>
        public TData Insert(TData data)
        {
            var result = Task.Run(()=> InsertAsync(data)).Result;
            return result;
        }

        /// <summary>
        /// Updates an existing item by applying a <see cref="Delta{T}"/> patch to it. The <see cref="Delta{T}"/>
        /// abstraction keeps track of which properties have changed which avoids problems with default values and
        /// the like.
        /// </summary>
        /// <param name="id">The id of the item to patch.</param>
        /// <param name="patch">The patch to apply.</param>
        /// <returns>The patched item.</returns>
        public TData Update(string id, Delta<TData> patch)
        {
            var result = Task.Run(() => UpdateAsync(id, patch)).Result;
            return result;
        }

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public TData Update(TData data)
        {
            var result = Task.Run(() => UpdateAsync(data)).Result;
            return result;
        }

        /// <summary>
        /// Completely replaces an existing item.
        /// </summary>
        /// <param name="id">The id of the item to replace.</param>
        /// <param name="data">The replacement</param>
        /// <returns>The replaced item</returns>
        public TData Replace(string id, TData data)
        {
            var result = Task.Run(() => ReplaceAsync(id, data)).Result;
            return result;
        }

        /// <summary>
        /// Deletes an existing item
        /// </summary>
        /// <param name="id">The id of the item to delete.</param>
        /// <returns><c>true</c> if item was deleted; otherwise <c>false</c></returns>
        public bool Delete(string id)
        {
            var result = Task.Run(() => DeleteAsync(id)).Result;
            return result;
        }

        /// <summary>
        /// Deletes an existing item
        /// </summary>
        /// <param name="id">The id of the item to delete.</param>
        /// <returns><c>true</c> if item was deleted; otherwise <c>false</c></returns>
        public bool Delete(TData data)
        {
            return Delete(data.Id);
        }

        #endregion

        #region Async Function

        /// <summary>
        /// Executes the provided <paramref name="query"/> against a store.
        /// </summary>
        /// <remarks>
        /// See also <see cref="M:LookupAsync"/> which is the companion method for executing a lookup for a single item.
        /// </remarks>
        /// <param name="query">The <see cref="ODataQueryOptions"/> query to execute.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> representing the result of the query.</returns>
        public override Task<IEnumerable<TData>> QueryAsync(ODataQueryOptions query)
        {
            if (Request == null)
                Request = CreateFakeRequest();

            var result = base.QueryAsync(query);
            return result;
        }

        /// <summary>
        /// Looks up a single item in the backend store. 
        /// </summary>
        /// <param name="id">The id representing the item. The id is provided as part of the <see cref="ITableData"/> and is visible to the client. 
        /// However, depending on the backend store and the domain model, the particular implementation may map the id to some other form of unique 
        /// identifier.</param>
        /// <remarks>
        /// See also <see cref="M:QueryAsync"/> which is the companion method for executing a query for multiple items.
        /// </remarks>
        /// <returns>A <see cref="SingleResult{T}"/> representing the result of the lookup. A <see cref="SingleResult{T}"/> represents an 
        /// <see cref="IQueryable"/> containing zero or one entities. This allows it to be composed with further querying such as <c>$select</c>.</returns>
        public override Task<SingleResult<TData>> LookupAsync(string id)
        {
            if (Request == null)
                Request = CreateFakeRequest();

            var result = base.LookupAsync(id);
            return result;
        }

        /// <summary>
        /// Inserts an item to the backend store.
        /// </summary>
        /// <param name="data">The data to be inserted</param>
        /// <returns>The inserted item.</returns>
        public override Task<TData> InsertAsync(TData data)
        {
            if (Request == null)
                Request = CreateFakeRequest();

            var result = base.InsertAsync(data);
            return result;
        }

        /// <summary>
        /// Updates an existing item by applying a <see cref="Delta{T}"/> patch to it. The <see cref="Delta{T}"/>
        /// abstraction keeps track of which properties have changed which avoids problems with default values and
        /// the like.
        /// </summary>
        /// <param name="id">The id of the item to patch.</param>
        /// <param name="patch">The patch to apply.</param>
        /// <returns>The patched item.</returns>
        public override Task<TData> UpdateAsync(string id, Delta<TData> patch)
        {
            if (Request == null)
                Request = CreateFakeRequest();

            return base.UpdateAsync(id, patch);
        }

        /// <summary>
        /// Completely replaces an existing item.
        /// </summary>
        /// <param name="id">The id of the item to replace.</param>
        /// <param name="item">The replacement</param>
        /// <returns>The replaced item</returns>
        public override Task<TData> ReplaceAsync(string id, TData item)
        {
            if (Request == null)
                Request = CreateFakeRequest();

            return base.ReplaceAsync(id, item);
        }

        /// <summary>
        /// Deletes an existing item
        /// </summary>
        /// <param name="id">The id of the item to delete.</param>
        /// <returns><c>true</c> if item was deleted; otherwise <c>false</c></returns>
        public override Task<bool> DeleteAsync(string id)
        {
            if (Request == null)
                Request = CreateFakeRequest();

            return base.DeleteAsync(id);
        }

        #endregion

        #region AsyncableFunction

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<TData> UpdateAsync(TData data)
        {
            //var id = data.Id;
            ////因為下面的方法不會更新資料，所以先刪除後新增
            //if (await DeleteAsync(id))
            //{
            //    data.Id = id;
            //    return await InsertAsync(data);
            //}            
            //return null;
            
            //先產生一個delta 物件
            var value = new Delta<TData>();
            value.Patch(data);

            //用原本azure 非同步方式同步
            return UpdateAsync(data.Id, value);
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<TData> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<TData> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual IDbSet<TData> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _dbContext.Set<TData>();
                return _entities;
            }
        }

        #endregion

        #region Private function

        private HttpRequestMessage CreateFakeRequest()
        {
            return new HttpRequestMessage();
        }

        protected override Task<int> SubmitChangesAsync()
        {
            try
            {
                if (IsRequestAssign)
                    return base.SubmitChangesAsync();

                //Create a fake task
                var result = ((DbContext)_dbContext).SaveChangesAsync();
                return result;
            }
            catch (DbEntityValidationException e)
            {
                string str = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    str= str + string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        str = str + string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }

                    Debug.WriteLine(str);
                }
                throw;
            }
            
        }

        #endregion
    }
}
