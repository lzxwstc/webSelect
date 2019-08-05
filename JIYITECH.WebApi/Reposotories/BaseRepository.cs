using Dapper;
using JIYITECH.WebApi.Entities;
using JIYITECH.WebApi.Configs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DiagnosticAdapter.Infrastructure;
using JIYITECH.WebApi.Services;

namespace JIYITECH.WebApi.Repositories
{
    public interface IBaseRepository<T>
    {
        #region  SynMethods
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">实例</param>
        /// <returns></returns>
        int? Add(T model);
        /// <summary>
        /// 获取头部第一条数据
        /// </summary>
        /// <returns></returns>
        T Head();
        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        int Delete(long id);
        /// <summary>
        /// 按条件删除数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        int DeleteList(string strWhere, object parameters);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">实例</param>
        /// <returns></returns>
        int Update(T model);
        /// <summary>
        /// 根据ID获取实体对象
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        T GetModel(long id);
        /// <summary>
        /// 根据条件获取实体对象集合
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        IEnumerable<T> GetModelList(string strWhere, object parameters);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="rowsPerPage">每页行数</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderBy">Order by排序</param>
        /// <param name="parameters">parameters参数</param>
        /// <returns>list 实体类列表，total 总条数</returns>
        object GetListPaged(int pageNumber, int rowsPerPage, string strWhere, string orderBy, object parameters = null);
        /// <summary>
        /// 根据条件分页获取实体对象集合总行数
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        int Count(string strWhere, object parameters = null);
        /// <summary>
        /// 条件检索
        /// </summary>
        /// <param name="strWhere">检索条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        T Get(string strWhere, object parameters = null);
        //Task<bool> UpdateFields(object parameters, IDbTransaction transaction = null, int? commandTimeout = null);
        #endregion
        #region  AsynMethods
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">实例</param>
        /// <returns></returns>
        Task<int?> AddAsync(T model);
        /// <summary>
        /// 获取头部第一条数据
        /// </summary>
        /// <returns></returns>
        Task<T> HeadAsync();
        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<int> DeleteAsync(long id);
        /// <summary>
        /// 按条件删除数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        Task<int> DeleteListAsync(string strWhere, object parameters);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">实例</param>
        /// <returns></returns>
        Task<int> UpdateAsync(T model);
        /// <summary>
        /// 根据ID获取实体对象
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<T> GetModelAsync(long id);
        /// <summary>
        /// 根据条件获取实体对象集合
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetModelListAsync(string strWhere, object parameters);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="rowsPerPage">每页行数</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderBy">Order by排序</param>
        /// <param name="parameters">parameters参数</param>
        /// <returns>list 实体类列表，total 总条数</returns>
        Task<object> GetListPagedAsync(int pageNumber, int rowsPerPage, string strWhere, string orderBy, object parameters = null);
        /// <summary>
        /// 根据条件分页获取实体对象集合总行数
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        Task<int> CountAsync(string strWhere, object parameters = null);
        Task<T> GetAsync(string strWhere, object parameters = null);
        //Task<bool> UpdateFields(object parameters, IDbTransaction transaction = null, int? commandTimeout = null);
        #endregion
    }
    /// <summary>
    /// 仓储层基类，通过泛型实现通用的CRUD操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : IBaseRepository<T>
    {
        private readonly AppConfig appConfig;
        string connstring;// erver = 172.17.13.13; database=smartcoal;user=yqrl;pwd=abcd@1234;";
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection db
        {
            get
            {
                return Transaction.Connection;
            }
        }

        public BaseRepository(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        #region  AsyncMethods
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">实例</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        public async Task<int?> AddAsync(T model)
        {
            return await db.InsertAsync<T>(model, transaction: Transaction);
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            return await db.DeleteAsync<T>(id, transaction: Transaction);
        }
        /// <summary>
        /// 按条件删除数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="parameters">参数</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        public async Task<int> DeleteListAsync(string strWhere, object parameters)
        {
            return await db.DeleteListAsync<T>(strWhere, parameters, transaction: Transaction);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">实例</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(T model)
        {
            model.GetType().GetProperty("updateTime").SetValue(model, DateTime.Now);
            return await db.UpdateAsync<T>(model, transaction: Transaction);
        }
        /// <summary>
        /// 根据ID获取实体对象
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<T> GetModelAsync(long id)
        {
            return await db.GetAsync<T>(id, transaction: Transaction);
        }

        /// <summary>
        /// 根据条件获取实体对象集合
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetModelListAsync(string strWhere, object parameters)
        {
            return await db.GetListAsync<T>(strWhere, parameters, transaction: Transaction);
        }
        /// <summary>
        /// 根据条件分页获取实体对象集合
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="rowsPerPage">每页行数</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderBy">Order by排序</param>
        /// <param name="parameters">parameters参数</param>
        /// <returns>list 实体类列表，total 总条数</returns>
        public async Task<object> GetListPagedAsync(int pageNumber, int rowsPerPage, string strWhere, string orderBy, object parameters = null)
        {
            var list = db.GetListPagedAsync<T>(pageNumber, rowsPerPage, strWhere, orderBy, parameters, transaction: Transaction);
            return new
            {
                total = CountAsync(strWhere),
                list = await list
            };
        }
        /// <summary>
        /// 根据条件分页获取实体对象集合总行数
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public async Task<int> CountAsync(string strWhere, object parameters = null)
        {
            return await db.RecordCountAsync<T>(strWhere, transaction: Transaction);
        }

        /// <summary>
        /// 获取头部第一条数据
        /// </summary>
        /// <returns></returns>
        public async Task<T> HeadAsync()
        {
            return await db.QueryFirstAsync<T>($"select top 1 * from [{typeof(T).Name}] order by id desc", transaction: Transaction);
        }
        #endregion

        #region  SyncMethods
        /// <summary>
        /// 增加一条数据
        /// </summary>
        /// <param name="model">实例</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        public int? Add(T model)
        {
            return db.Insert<T>(model, transaction: Transaction);
        }

        /// <summary>
        /// 根据ID删除一条数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        public int Delete(long id)
        {
            return db.Delete<T>(id, transaction: Transaction);
        }
        /// <summary>
        /// 按条件删除数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int DeleteList(string strWhere, object parameters)
        {
            return db.DeleteList<T>(strWhere, parameters, transaction: Transaction);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model">实例</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        public int Update(T model)
        {
            model.GetType().GetProperty("updateTime").SetValue(model, DateTime.Now);
            return db.Update<T>(model, transaction: Transaction);
        }
        /// <summary>
        /// 根据ID获取实体对象
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public T GetModel(long id)
        {
            return db.Get<T>(id, transaction: Transaction);
        }

        /// <summary>
        /// 根据条件获取实体对象集合
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public IEnumerable<T> GetModelList(string strWhere, object parameters)
        {
            return db.GetList<T>(strWhere, parameters, transaction: Transaction);
        }
        /// <summary>
        /// 根据条件分页获取实体对象集合
        /// </summary>
        /// <param name="pageNumber">页码</param>
        /// <param name="rowsPerPage">每页行数</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderBy">Order by排序</param>
        /// <param name="parameters">parameters参数</param>
        /// <returns>list 实体类列表，total 总条数</returns>
        public object GetListPaged(int pageNumber, int rowsPerPage, string strWhere, string orderBy, object parameters = null)
        {
            var list = db.GetListPaged<T>(pageNumber, rowsPerPage, strWhere, orderBy, parameters, transaction: Transaction);
            return new
            {
                total = Count(strWhere),
                list
            };
        }
        /// <summary>
        /// 根据条件分页获取实体对象集合总行数
        /// </summary>
        /// <param name="strWhere">条件语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int Count(string strWhere, object parameters = null)
        {
            return db.RecordCount<T>(strWhere, transaction: Transaction);
        }

        /// <summary>
        /// 获取头部第一条数据
        /// </summary>
        /// <returns></returns>
        public T Head()
        {
            var tableAttribute = (TableAttribute)(typeof(T).GetCustomAttributes(typeof(TableAttribute), true)[0]);
            return db.QueryFirst<T>($"select top 1 * from [{tableAttribute.Name}] order by id desc", transaction: Transaction);
        }

        //public bool UpdateGiven(T model, IDbTransaction transaction = null, int? commandTimeout = null)
        //{
        //    using (db = new SqlConnection(connstring))
        //    {
        //        if (transaction == null)
        //        {
        //            return Dapper.Contrib.Extensions.SqlMapperExtensions.UpdateFields<T>(db, model);
        //        }
        //        else
        //        {
        //            return Dapper.Contrib.Extensions.SqlMapperExtensions.UpdateFields<T>(db, model,transaction);
        //        }
        //    }
        //}
        #endregion



        //// 实现接口方法
        //// 由类的使用者，在外部显示调用，释放类资源
        //public void Dispose()
        //{
        //    // 释放托管和非托管资源
        //    // 将对象从垃圾回收器链表中移除，
        //    // 从而在垃圾回收器工作时，只释放托管资源，而不执行此对象的析构函数
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //// 析构，由垃圾回收器调用，释放非托管资源
        //~BaseRepository()
        //{
        //    // 释放非托管资源
        //    Dispose(false);
        //}
        //// 参数为true表示释放所有资源，只能由使用者调用
        //// 参数为false表示释放非托管资源，只能由垃圾回收器自动调用
        //// 如果子类有自己的非托管资源，可以重载这个函数，添加自己的非托管资源的释放
        //// 但是要记住，重载此函数必须保证调用基类的版本，以保证基类的资源正常释放
        //protected virtual void Dispose(bool disposing)
        //{
        //    // 如果资源未释放 这个判断主要用了防止对象被多次释放
        //    if (!this.isDisposed)
        //    {
        //        if (disposing)
        //        {
        //            // 释放托管资源
        //            db.Dispose();
        //        }
        //    }
        //    // 标识此对象已释放
        //    this.isDisposed = true;
        //}

        public T Get(string strWhere, object parameters = null)
        {
            return db.QuerySingle<T>($"select top 1 * from  [{typeof(T).Name}] {strWhere}", transaction: Transaction);
        }


        public async Task<T> GetAsync(string strWhere, object parameters = null)
        {
            return await db.QuerySingleAsync<T>($"select top 1 * from  [{typeof(T).Name}] {strWhere}", transaction: Transaction);
        }
    }
}
