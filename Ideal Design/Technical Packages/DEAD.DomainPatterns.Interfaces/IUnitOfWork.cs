using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DEAD.DomainPatterns
{
	public interface IUnitOfWork : IDisposable
	{

		object GetUOWCore();
		ITransaction BeginTransaction();
		ITransaction BeginTransaction(System.Data.IsolationLevel level);


		IRepository<T> GetRepository<T>() where T : class;
		Task<IRepository<T>> GetRepositoryAsync<T>(CancellationToken cancellationToken = default(CancellationToken)) where T : class;


		IRepository<T> GetRepositoryForEntity<T>(T entity) where T : class;
		Task<IRepository<T>> GetRepositoryForEntityAsync<T>(T entity, CancellationToken cancellationToken = default(CancellationToken)) where T : class;

		IRepository<T> GetRepositoryForEntities<T>(IEnumerable<T> entites) where T : class;
		Task<IRepository<T>> GetRepositoryForEntitiesAsync<T>(IEnumerable<T> entites, CancellationToken cancellationToken = default(CancellationToken)) where T : class;



		TRepository GetRepository<T, TRepository>()
			where T : class
			where TRepository : IRepository<T>;
		Task<TRepository> GetRepositoryAsync<T, TRepository>(CancellationToken cancellationToken = default(CancellationToken))
			where T : class
			where TRepository : IRepository<T>;


		int Submit();
		Task<int> SubmitAsync(CancellationToken cancellationToken = default (CancellationToken));


		DataSet ExecuteDataSet(string sqlQuery, params DbParameter[] parameters);
		Task<DataSet> ExecuteDataSetAsync(string sqlQuery, params DbParameter[] parameters);
		IEnumerable<TEntity> SqlQuery<TEntity>(string sqlQuery, params DbParameter[] parameters);
		DbDataReader SqlQueryDataReader(string sqlQuery, params DbParameter[] parameters);
		Task<DbDataReader> SqlQueryDataReaderAsync(string sqlQuery, params DbParameter[] parameters);
		IEnumerable<TDictEntity> SqlQueryDynamically<TDictEntity>(string sqlQuery, params DbParameter[] parameters) where TDictEntity : IDictionary<string, object>, new();
		Task<IEnumerable<TDictEntity>> SqlQueryDynamicallyAsync<TDictEntity>(string sqlQuery, params DbParameter[] parameters) where TDictEntity : IDictionary<string, object>, new();


	}
}
