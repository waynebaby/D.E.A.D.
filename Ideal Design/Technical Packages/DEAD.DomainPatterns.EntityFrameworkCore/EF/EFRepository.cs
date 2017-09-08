using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
        //protected ObjectContext _objectContext;




        /// <summary>
        /// 创建一个 <see cref="EFRepository{T}"/> class 的新实例.
        /// </summary>
        /// <param name="uow">The unit of work(工作单元).</param>
        public EFRepository(EFUnitOfWork uow)
        {
            UnitOfWork = uow;
            _dbSet = UnitOfWork.Context.Set<T>();
            //_objectContext = (UnitOfWork.Context as System.Data.Entity.Infrastructure.IObjectContextAdapter).ObjectContext;
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
            _dbSet.Add(item);
            return item;
        }
        public IEnumerable<T> AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                return Enumerable.Empty<T>();
            }

            _dbSet.AddRange(items);
            return items;
        }

        public T Remove(T item)
        {
            _dbSet.Remove(item);
            return item;

        }
        public IEnumerable<T> RemoveRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                return Enumerable.Empty<T>();
            }
            _dbSet.RemoveRange(items);
            return items;
        }



        public T Detach(T item)
        {
            var entry = UnitOfWork.Context.Entry(item);
            entry.State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            return item;
        }





        public EntityState GetEntityState(T item)
        {
            return (EntityState)(int)UnitOfWork.Context.Entry(item).State;
        }




        public T AttachAndCancelChanges(T item)
        {
            var entry = _dbSet.Attach(item);
            entry.Reload();
            item = entry.Entity;
            return item;
        }

        public async Task<T> AttachAndCancelChangesAsync(T item, CancellationToken cancellationToken)
        {
            var entry = _dbSet.Attach(item);
            await entry.ReloadAsync(cancellationToken);
            item = entry.Entity;
            return item;
        }

        public T[] AttachAndCancelChanges(params T[] items)
        {
            _dbSet.AttachRange(items);
            var es = items.Select(item => UnitOfWork.Context.Entry(item));
            foreach (var entry in es)
            {
                entry.Reload();
            }
            return es.Select(x => x.Entity).ToArray();
        }

        public async Task<T[]> AttachAndCancelChangesAsync(T[] items, CancellationToken cancellationToken)
        {
            _dbSet.AttachRange(items);
            var es = items.Select(item => UnitOfWork.Context.Entry(item));
            foreach (var entry in es)
            {
                await entry.ReloadAsync();
            }
            return es.Select(x => x.Entity).ToArray();
        }


        public T AttachAndMarkChangesOrAdd(T item)
        {
            var entry = _dbSet.Attach(item);
            if (entry.GetDatabaseValues() != null)
            {
                UnitOfWork.Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                Add(item);
            }
            return item;
        }

        public T[] AttachAndMarkChangesOrAdd(params T[] items)
        {
            items = items.Select(item =>
            {
                var entry = _dbSet.Attach(item);
                if (entry.GetDatabaseValues() != null)
                {
                    UnitOfWork.Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                else
                {
                    Add(item);
                }
                return item;
            }).ToArray();
            return items;
        }



        public async Task<T> AttachAndMarkChangesOrAddAsync(T item)
        {
            var entry = _dbSet.Attach(item);
            if (entry.GetDatabaseValues() != null)
            {
                UnitOfWork.Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                await _dbSet.AddAsync(item);
            }
            return item;
        }

        public async Task<T[]> AttachAndMarkChangesOrAddAsync(params T[] items)
        {
            var tsks = items.Select(item =>
             {
                 return new Func<Task<T>>(async () =>
                 {
                     var entry = _dbSet.Attach(item);
                     if (entry.GetDatabaseValues() != null)
                     {
                         UnitOfWork.Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                     }
                     else
                     {
                         await _dbSet.AddAsync(item);
                     }
                     return item;
                 });
             });
            var lst = new List<T>();
            foreach (var task in tsks)
            {
                lst.Add(await task());
            }

            return lst.ToArray();
        }

    }
}
