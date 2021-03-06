﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Reflection;
using DEAD.DomainPatterns;
using System.Threading;

namespace DEAD.DomainPatterns.EFCore
{
	/// <summary>
	/// 资料库
	/// </summary>
	/// <typeparam name="T">资料类型</typeparam>
	public class EFRepository<T> : IRepository<T> where T : class
	{
		protected DbSet<T> _dbSet;
		protected ObjectContext _objectContext;




		/// <summary>
		/// 创建一个 <see cref="EFRepository{T}"/> class 的新实例.
		/// </summary>
		/// <param name="uow">The unit of work(工作单元).</param>
		public EFRepository(EFUnitOfWork uow)
		{
			UnitOfWork = uow;
			_dbSet = UnitOfWork.Context.Set<T>();
			_objectContext = (UnitOfWork.Context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
		}

		/// <summary>
		/// 获取 the unit of work 工作单元.
		/// </summary>
		/// <value>
		/// The unit of work.
		/// </value>
		public EFUnitOfWork UnitOfWork
		{
			get;
			protected set;

		}

		/// <summary>
		/// Gets the querable root.	这个资料库的可查询根
		/// </summary>
		/// <returns></returns>
		protected virtual IQueryable<T> GetQuerableRoot()
		{
			return UnitOfWork.Context.Set<T>();
		}

		/// <summary>
		/// Gets the unit of work.
		/// </summary>
		/// <value>
		/// The unit of work.
		/// </value>
		IUnitOfWork IRepository<T>.UnitOfWork
		{
			get { return UnitOfWork; }
		}

		/// <summary>
		/// 跟踪状态的查询入口
		/// </summary>
		/// <value>
		/// The entities.
		/// </value>
		public IQueryable<T> Entities
		{
			get { return GetQuerableRoot(); }
		}

		/// <summary>
		/// 不跟踪状态的查询入口
		/// </summary>
		/// <value>
		/// The untracked entities.
		/// </value>
		public IQueryable<T> UntrackedEntities
		{
			get { return GetQuerableRoot().AsNoTracking(); }
		}

		public T Add(T item)
		{
			return _dbSet.Add(item);
		}
		public IEnumerable<T> AddRange(IEnumerable<T> items)
		{
			if (items == null)
			{
				return Enumerable.Empty<T>();
			}

			return _dbSet.AddRange(items);
		}

		public T Remove(T item)
		{
			return _dbSet.Remove(item);
		}
		public IEnumerable<T> RemoveRange(IEnumerable<T> items)
		{
			if (items == null)
			{
				return Enumerable.Empty<T>();
			}
			return _dbSet.RemoveRange(items);
		}



		public T Detach(T item)
		{
			_objectContext.Detach(item);
			return item;
		}

        

		public EntityState GetEntityState(T item)
		{
			return (EntityState)(int)UnitOfWork.Context.Entry(item).State;
		}




		public T AttachAndCancelChanges(T item)
		{
			item = _dbSet.Attach(item);
			_objectContext.Refresh(RefreshMode.StoreWins, item);
			return item;
		}

		public async Task<T> AttachAndCancelChangesAsync(T item, CancellationToken cancellationToken)
		{
			item = _dbSet.Attach(item);
			await _objectContext.RefreshAsync(RefreshMode.StoreWins, item, cancellationToken);
			return item;
		}

		public T[] AttachAndCancelChanges(params T[] items)
		{
			items = items.Select(x => _dbSet.Attach(x)).ToArray();
			_objectContext.Refresh(RefreshMode.StoreWins, items);
			return items;
		}

		public async Task<T[]> AttachAndCancelChangesAsync(T[] items, CancellationToken cancellationToken)
		{
			items = items.Select(x => _dbSet.Attach(x)).ToArray();
			await _objectContext.RefreshAsync(RefreshMode.StoreWins, items, cancellationToken);
			return items;
		}


		public T AttachAndMarkChangesOrAdd(T item)
		{
			item = _dbSet.Attach(item);
			if (UnitOfWork.Context.Entry(item).GetDatabaseValues() != null)
			{
				UnitOfWork.Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
			}
			else
			{
				Add(item);
			}
			return item;
		}

		public T[] AttachAndMarkChangesOrAdd(params T[] items)
		{
			items = items.Select(x =>
			{
				_dbSet.Attach(x);
				if (UnitOfWork.Context.Entry(x).GetDatabaseValues() != null)
				{
					UnitOfWork.Context.Entry(x).State = System.Data.Entity.EntityState.Modified;
				}
				else
				{
					Add(x);
				}
				return x;
			}).ToArray();
			return items;
		}



		public async Task<T> AttachAndMarkChangesOrAddAsync(T item)
		{
			item = _dbSet.Attach(item);
			if ( null!= await UnitOfWork.Context.Entry(item).GetDatabaseValuesAsync())
			{
				UnitOfWork.Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
			}
			else
			{
				Add(item);
			}
			return item;
		}

		public async Task<T[]> AttachAndMarkChangesOrAddAsync(params T[] items)
		{
			foreach (var x in items)
			{
				_dbSet.Attach(x);
				if (null!= await UnitOfWork.Context.Entry(x).GetDatabaseValuesAsync())
				{
					UnitOfWork.Context.Entry(x).State = System.Data.Entity.EntityState.Modified;
				}
				else
				{
					Add(x);
				}
			}
			return items;
		}

	}
}
