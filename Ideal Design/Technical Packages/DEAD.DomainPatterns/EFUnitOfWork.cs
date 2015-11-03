
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DEAD.DomainPatterns.EF
{
	public class EFUnitOfWork : IUnitOfWork
	{
		private DbContext _context;
		private ObjectContext _objectContext;


		public DbContext Context
		{
			get { return _context; }
			private set
			{
				_context = value;
				_objectContext = (value as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;

			}
		}


		public EFUnitOfWork(DbContext context)
		{
			_context = context;
		}




		public ITransaction BeginTransaction()
		{
			return new EFTransaction(this);
		}

		public ITransaction BeginTransaction(System.Data.IsolationLevel level)
		{
			return new EFTransaction(this, level);
		}
		public virtual IRepository<T> GetRepository<T>() where T : class
		{
			return new EFRepository<T>(this);
		}

		public virtual async Task<IRepository<T>> GetRepositoryAsync<T>(System.Threading.CancellationToken cancellationToken) where T : class
		{
			return await Task.FromResult(new EFRepository<T>(this));
		}

		protected virtual void OnSubmit()
		{
			
		}

		public int Submit()
		{	   

			try
			{
				OnSubmit();
				return Context.SaveChanges();

			}
			catch (System.Data.Entity.Validation.DbEntityValidationException ex)
			{
				throw;

			}
			catch (System.Data.Entity.Validation.DbUnexpectedValidationException ex)
			{
				throw;
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		public async Task<int> SubmitAsync(System.Threading.CancellationToken cancellationToken)
		{
			try
			{
				OnSubmit();
				return await Context.SaveChangesAsync(cancellationToken);
			}
			catch (System.Data.Entity.Validation.DbEntityValidationException ex)
			{
				var sb = new StringBuilder();

				sb.AppendLine(ex.ToString());
				foreach (var item in ex.EntityValidationErrors)
				{
					sb.AppendLine("-----------");
					sb.AppendLine(item.Entry.Entity.GetType().Name);
					foreach (var item2 in item.ValidationErrors)
					{
						sb.AppendFormat("{0}:{1}", item2.PropertyName, item2.ErrorMessage).AppendLine();
					}
				}

				var er = sb.ToString();
				Debug.WriteLine(sb.ToString());

#if DEBUG
				throw new Exception(er, ex);

#else
				throw ;

#endif


			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.ToString());
				throw;
			}
		}

		public void Dispose()
		{

			Dispose(true);
		}

		protected void Dispose(bool disposing)
		{
			if (disposing)
			{

				_context.Dispose();
				GC.SuppressFinalize(this);
			}
		}

		public virtual TRepository GetRepository<T, TRepository>()
			where T : class
			where TRepository : IRepository<T>
		{
			throw new NotImplementedException();
		}

		public virtual Task<TRepository> GetRepositoryAsync<T, TRepository>(System.Threading.CancellationToken cancellationToken)
			where T : class
			where TRepository : IRepository<T>
		{
			throw new NotImplementedException();
		}


		public IRepository<T> GetRepositoryForEntity<T>(T entity) where T : class
		{
			return GetRepository<T>();
		}

		public Task<IRepository<T>> GetRepositoryForEntityAsync<T>(T entity, System.Threading.CancellationToken cancellationToken = default(CancellationToken)) where T : class
		{
			return GetRepositoryAsync<T>(cancellationToken);
		}

		public IRepository<T> GetRepositoryForEntities<T>(IEnumerable<T> entities) where T : class
		{
			return GetRepository<T>();
		}

		public Task<IRepository<T>> GetRepositoryForEntitiesAsync<T>(IEnumerable<T> entities, System.Threading.CancellationToken cancellationToken = default(CancellationToken)) where T : class
		{
			return GetRepositoryAsync<T>(cancellationToken);
		}

		public object GetUOWCore()
		{
			return this.Context;
		}
	}
}
