
using DEAD.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DEAD.DomainPatterns.EFCore
{
	public class EFUnitOfWork : IUnitOfWork, IEFUnitOfWork
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



		public object GetUOWCore()
		{
			return this.Context;
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


		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public System.Data.DataSet ExecuteDataSet(string sqlQuery, params System.Data.Common.DbParameter[] parameters)
		{


			var cmd = _context.Database.Connection.CreateCommand();
			cmd.CommandText = sqlQuery;
			cmd.Parameters.AddRange(parameters);
			//http://forums.asp.net/t/1452859.aspx  multi recordset cannot get auto table name;

			var ds = new DataSet();

			var oldstate = cmd.Connection.State;

			if (oldstate != System.Data.ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			using (var rder = cmd.ExecuteReader())
			{
				var index = 0;
				do
				{


					var t = new DataTable("Table" + (index == 0 ? "" : index.ToString()));
					//var tsm = rder.GetSchemaTable();


					t.BeginLoadData();
					for (int i = 0; i < rder.FieldCount; i++)
					{
						t.Columns.Add(new DataColumn(rder.GetName(i), rder.GetFieldType(i)));
					}
					while (rder.Read())
					{
						var r = t.NewRow();
						r.BeginEdit();
						for (int i = 0; i < rder.FieldCount; i++)
						{
							//(rder.GetName(i), rder.GetValue(i));
							var n = rder.GetName(i);
							r[n] = rder[n];
						}
						t.Rows.Add(r);
						r.EndEdit();
					}
					t.AcceptChanges();
					t.EndLoadData();
					ds.Tables.Add(t);

					index++;
				} while (rder.NextResult());

			}

			if (oldstate != cmd.Connection.State)
			{
				if (oldstate == System.Data.ConnectionState.Closed)
				{
					cmd.Connection.Close();
				}
			}

			return ds;

		}

		public async Task<System.Data.DataSet> ExecuteDataSetAsync(string sqlQuery, params System.Data.Common.DbParameter[] parameters)
		{


			var cmd = _context.Database.Connection.CreateCommand();
			cmd.CommandText = sqlQuery;
			cmd.Parameters.AddRange(parameters);
			var oldstate = cmd.Connection.State;


			//http://forums.asp.net/t/1452859.aspx  multi recordset cannot get auto table name;

			var ds = new DataSet();


			if (oldstate != System.Data.ConnectionState.Open)
			{
				await cmd.Connection.OpenAsync();
			}
			using (var rder = await cmd.ExecuteReaderAsync())
			{
				int index = 0;
				do
				{

					var t = new DataTable("Table" + (index == 0 ? "" : index.ToString()));
					//var tsm = rder.GetSchemaTable();	   
					t.BeginLoadData();
					for (int i = 0; i < rder.FieldCount; i++)
					{
						t.Columns.Add(new DataColumn(rder.GetName(i), rder.GetFieldType(i)));
					}

					while (await rder.ReadAsync())
					{
						var r = t.NewRow();
						r.BeginEdit();
						for (int i = 0; i < rder.FieldCount; i++)
						{
							//(rder.GetName(i), rder.GetValue(i));
							var n = rder.GetName(i);
							r[n] = rder[n];
						}
						t.Rows.Add(r);
						r.EndEdit();

					}
					t.AcceptChanges();
					t.EndLoadData();
					ds.Tables.Add(t);
					index++;
				} while (await rder.NextResultAsync());

			}

			if (oldstate != cmd.Connection.State)
			{
				if (oldstate == System.Data.ConnectionState.Closed)
				{
					cmd.Connection.Close();
				}
			}

			return ds;

		}


		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public IEnumerable<TDictEntity> SqlQueryDynamically<TDictEntity>(string sqlQuery, params DbParameter[] parameters) where TDictEntity : IDictionary<string, Object>, new()
		{
			var cmd = _context.Database.Connection.CreateCommand();
			cmd.CommandText = sqlQuery;
			cmd.Parameters.AddRange(parameters);
			var oldstate = cmd.Connection.State;

			if (oldstate != System.Data.ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			using (var rder = cmd.ExecuteReader())
			{

				do
				{


					while (rder.Read())
					{
						var dic = new TDictEntity();
						for (int i = 0; i < rder.FieldCount; i++)
						{
							dic.Add(rder.GetName(i), rder.GetValue(i));
						}
						yield return dic;
					}
				} while (rder.NextResult());

			}

			if (oldstate != cmd.Connection.State)
			{
				if (oldstate == System.Data.ConnectionState.Closed)
				{
					cmd.Connection.Close();
				}
			}
		}

		public async Task<IEnumerable<TDictEntity>> SqlQueryDynamicallyAsync<TDictEntity>(string sqlQuery, params DbParameter[] parameters) where TDictEntity : IDictionary<string, Object>, new()
		{

			var list = new List<TDictEntity>();
			var cmd = _context.Database.Connection.CreateCommand();
			cmd.CommandText = sqlQuery;
			cmd.Parameters.AddRange(parameters);

			var oldstate = cmd.Connection.State;

			if (oldstate != System.Data.ConnectionState.Open)
			{
				await cmd.Connection.OpenAsync();
			}


			using (var rder = await cmd.ExecuteReaderAsync())
			{
				do
				{
					while (await rder.ReadAsync())
					{
						var dic = new TDictEntity();
						for (int i = 0; i < rder.FieldCount; i++)
						{
							dic.Add(rder.GetName(i), rder.GetValue(i));
						}
						list.Add(dic);
					}
				} while (await rder.NextResultAsync());

			}

			if (oldstate != cmd.Connection.State)
			{
				if (oldstate == System.Data.ConnectionState.Closed)
				{
					cmd.Connection.Close();
				}
			}
			return list;
		}





		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public DbDataReader SqlQueryDataReader(string sqlQuery, params DbParameter[] parameters)
		{
			var cmd = _context.Database.Connection.CreateCommand();
			cmd.CommandText = sqlQuery;
			cmd.Parameters.AddRange(parameters);
			var oldstate = cmd.Connection.State;

			if (oldstate != System.Data.ConnectionState.Open)
			{
				cmd.Connection.Open();
			}
			var rder = cmd.ExecuteReader();

			return rder;

		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
		public async Task<DbDataReader> SqlQueryDataReaderAsync(string sqlQuery, params DbParameter[] parameters)
		{
			var cmd = _context.Database.Connection.CreateCommand();
			cmd.CommandText = sqlQuery;
			cmd.Parameters.AddRange(parameters);
			var oldstate = cmd.Connection.State;

			if (oldstate != System.Data.ConnectionState.Open)
			{
				await cmd.Connection.OpenAsync();
			}
			var rder = await cmd.ExecuteReaderAsync();

			return rder;

		}

         IEnumerable<TEntity>  IUnitOfWork.SqlQuery<TEntity>(string sqlQuery, params DbParameter[] parameters)
   {

            return _context.Database.SqlQuery<TEntity>(sqlQuery, parameters).AsQueryable();
        }

        IEnumerable<TEntity> IEFUnitOfWork.SqlQuery<TEntity>(string sqlQuery, params DbParameter[] parameters)
        {

            return _context.Database.SqlQuery<TEntity>(sqlQuery, parameters).AsQueryable();
        }

        //public PagedCollection<TEntity> SqlQueryPaged<TEntity>(string sqlQuery, DbParameter[] parameters, List<SortDirection> orderBy, int pageIndex, int pageSize, int? totalCount = default(int?))
        //{
        //	List<TEntity> items = null;
        //	string sqlTemplate = @" ORDER BY {0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY";
        //	StringBuilder sbSort = new StringBuilder();
        //	if (orderBy != null)
        //	{
        //		int i = 0;
        //		foreach (var item in orderBy)
        //		{
        //			if (i != 0) sbSort.Append(", ");

        //			//sbSort.Append(item.Member);
        //			sbSort.Append(SecurityHelper.FilterSqlInjectChart(item.Member));

        //			if (item.Direction == Direction.Descending)
        //			{
        //				sbSort.Append(" ").Append("DESC");
        //			}
        //			i++;
        //		}
        //	}

        //	string sql = sqlQuery + string.Format(sqlTemplate, sbSort.ToString(), ((pageIndex - 1) * pageSize), pageSize);

        //	items = _context.Database.SqlQuery<TEntity>(sql, parameters).ToList();

        //	if (!totalCount.HasValue)
        //	{
        //		sqlTemplate = @"SELECT COUNT(1) AS TotalCount FROM ({0}) AS TCount";
        //		sql = string.Format(sqlTemplate, sqlQuery);

        //		SqlParameter[] newParamters = new SqlParameter[0];
        //		if (parameters != null && parameters.Length > 0)
        //		{
        //			List<SqlParameter> paraList = null;
        //			paraList = new List<SqlParameter>();
        //			foreach (SqlParameter item in parameters)
        //			{
        //				SqlParameter parm = new SqlParameter(item.ParameterName, item.Value);
        //				paraList.Add(parm);
        //			}

        //			newParamters = paraList.ToArray();
        //		}

        //		totalCount = _context.Database.SqlQuery<int>(sql, newParamters).First();
        //	}

        //	return new PagedCollection<TEntity> { PageIndex = pageIndex, PageSize = pageSize, Items = items, TotalItems = totalCount.Value };
        //}
    }
}
