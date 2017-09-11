using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace DEAD.DomainPatterns.EFCore
{
	public interface IEFUnitOfWork
	{
		DataSet ExecuteDataSet(string sqlQuery, params DbParameter[] parameters);
		Task<DataSet> ExecuteDataSetAsync(string sqlQuery, params DbParameter[] parameters);
		IEnumerable<TEntity> SqlQuery<TEntity>(string sqlQuery, params DbParameter[] parameters) where TEntity:class;
		DbDataReader SqlQueryDataReader(string sqlQuery, params DbParameter[] parameters);
		Task<DbDataReader> SqlQueryDataReaderAsync(string sqlQuery, params DbParameter[] parameters);
		IEnumerable<TDictEntity> SqlQueryDynamically<TDictEntity>(string sqlQuery, params DbParameter[] parameters) where TDictEntity : IDictionary<string, object>, new();
		Task<IEnumerable<TDictEntity>> SqlQueryDynamicallyAsync<TDictEntity>(string sqlQuery, params DbParameter[] parameters) where TDictEntity : IDictionary<string, object>, new();
	}
}