using System;
using System.Collections.Generic;
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
		//ITransaction BeginTransaction(System.Data.IsolationLevel level);


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

	}
}
