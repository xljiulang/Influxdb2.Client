using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// IInfuxdb实现
    /// </summary>
    sealed class InfuxdbImpl : IInfuxdb
    {
        private readonly InfuxdbClient infuxdbClient;

        /// <summary>
        /// IInfuxdb实现
        /// </summary>
        /// <param name="infuxdbClient"></param>
        public InfuxdbImpl(InfuxdbClient infuxdbClient)
        {
            this.infuxdbClient = infuxdbClient;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        public Task<TModel[]> QueryAsync<TModel>(IFlux flux, string? org = default) where TModel : new()
        {
            return this.QueryAsync<TModel>(flux.ToString(), org);
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="flux">flux表达式</param>
        /// <param name="org">组织</param>
        /// <returns></returns>
        public async Task<TModel[]> QueryAsync<TModel>(string flux, string? org = default) where TModel : new()
        {
            var tables = await this.QueryAsync(flux, org);
            tables.EnsureNoError();

            return tables.Count == 1
                ? tables[0].ToModels<TModel>()
                : tables.SelectMany(item => item.ToModels<TModel>()).ToArray();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public Task<IDataTableCollection> QueryAsync(IFlux flux, string? org = null)
        {
            return this.QueryAsync(flux.ToString(), org);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public async Task<IDataTableCollection> QueryAsync(string flux, string? org = null)
        {
            using var content = new FluxContent(flux);
            return await this.infuxdbClient.QueryAsync(content, org);
        }

        /// <summary>
        /// 写入实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="bucket"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public Task<int> WriteAsync<TEntity>(TEntity entity, string? bucket = null, string? org = null) where TEntity : notnull
        {
            var point = new Point<TEntity>(entity);
            return this.WritePointAsync(point);
        }

        /// <summary>
        /// 写入实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="bucket"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public Task<int> WriteAsync<TEntity>(IEnumerable<TEntity> entities, string? bucket = null, string? org = null) where TEntity : notnull
        {
            var points = entities.Select(e => new Point<TEntity>(e));
            return this.WritePointAsync(points, bucket, org);
        }

        /// <summary>
        /// 写入数据点
        /// </summary>
        /// <param name="point"></param>
        /// <param name="bucket"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public Task<int> WritePointAsync(IPoint point, string? bucket = null, string? org = null)
        {
            var points = new[] { point };
            return this.WritePointAsync(points, bucket, org);
        }

        /// <summary>
        /// 写入数据点
        /// </summary>
        /// <param name="points"></param>
        /// <param name="bucket"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public async Task<int> WritePointAsync(IEnumerable<IPoint> points, string? bucket = null, string? org = null)
        {
            var count = points.Count();
            if (count == 0)
            {
                return 0;
            }

            var index = 0;
            using var content = new LineProtocolContent();
            foreach (var point in points)
            {
                point.WriteTo(content);
                if (++index < count)
                {
                    content.WriteLine();
                }
            }

            await this.infuxdbClient.WriteAsync(content, bucket, org);
            return count;
        }
    }
}
