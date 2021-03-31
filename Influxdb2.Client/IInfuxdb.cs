using System.Collections.Generic;
using System.Threading.Tasks;

namespace Influxdb2.Client
{
    /// <summary>
    /// Infuxdb操作的接口
    /// </summary>
    public interface IInfuxdb
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        Task<TModel[]> QueryAsync<TModel>(IFlux flux, string? org = default) where TModel : new();

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        Task<TModel[]> QueryAsync<TModel>(string flux, string? org = default) where TModel : new();


        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        Task<IDataTableCollection> QueryAsync(IFlux flux, string? org = default);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        Task<IDataTableCollection> QueryAsync(string flux, string? org = default);



        /// <summary>
        /// 写入实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">ColumnTypeAttribute标记的实体</param>
        /// <param name="bucket">空间名</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        Task<int> WriteAsync<TEntity>(TEntity entity, string? bucket = default, string? org = default) where TEntity : notnull;

        /// <summary>
        /// 写入实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities">ColumnTypeAttribute标记的实体</param>
        /// <param name="bucket">空间名</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        Task<int> WriteAsync<TEntity>(IEnumerable<TEntity> entities, string? bucket = default, string? org = default) where TEntity : notnull;



        /// <summary>
        /// 写入数据点
        /// </summary>
        /// <param name="entity">数据点</param>
        /// <param name="bucket">空间名</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        Task<int> WritePointAsync(IPoint point, string? bucket = default, string? org = default);


        /// <summary>
        /// 写入数据点
        /// </summary>
        /// <param name="points">数据点</param>
        /// <param name="bucket">空间名</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        Task<int> WritePointAsync(IEnumerable<IPoint> points, string? bucket = default, string? org = default);
    }
}
