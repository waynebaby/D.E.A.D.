using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DEAD.DomainPatterns
{
	/// <summary>
	/// Repository
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IRepository<T> where T : class
	{
		/// <summary>
		/// Gets the unit of work.
		/// </summary>
		/// <value>
		/// The unit of work.
		/// </value>
		IUnitOfWork UnitOfWork { get; }
		/// <summary>
		/// Gets the entities.
		/// </summary>
		/// <value>
		/// The entities.
		/// </value>
		IQueryable<T> Entities { get; }
		/// <summary>
		/// Gets the un tracked entities.
		/// </summary>
		/// <value>
		/// The un tracked entities.
		/// </value>
		IQueryable<T> UntrackedEntities { get; }
		T Add(T item);

		/// <summary>
		/// Adds the collection. collection could be null;
		/// </summary>
		/// <param name="items">The items.</param>
		/// <returns></returns>
		IEnumerable<T> AddRange(IEnumerable<T> items);
		T Remove(T item);
		/// <summary>
		/// Removes the collection. collection could be null
		/// </summary>
		/// <param name="items">The items.</param>
		/// <returns></returns>
		IEnumerable<T> RemoveRange(IEnumerable<T> items);

		T AttachAndMarkChangesOrAdd(T item);

		T[] AttachAndMarkChangesOrAdd(params T[] items);

		Task<T> AttachAndMarkChangesOrAddAsync(T item);

		Task<T[]> AttachAndMarkChangesOrAddAsync(params T[] items);

		T AttachAndCancelChanges(T item);
		Task<T> AttachAndCancelChangesAsync(T item, CancellationToken cancellationToken = default(CancellationToken)); 
		T[] AttachAndCancelChanges(params T[] items);
		Task<T[]> AttachAndCancelChangesAsync(T[] items, CancellationToken cancellationToken = default(CancellationToken));

		T Detach(T item);

		EntityState GetEntityState(T item);

	}

	// Summary:
	//     Describes the state of an entity.
	[Flags]
	public enum EntityState
	{
		// Summary:
		//     The entity is not being tracked by the context.  An entity is in this state
		//     immediately after it has been created with the new operator or with one of
		//     the System.Data.Entity.DbSet Create methods.
		Detached = 1,
		//
		// Summary:
		//     The entity is being tracked by the context and exists in the database, and
		//     its property values have not changed from the values in the database.
		Unchanged = 2,
		//
		// Summary:
		//     The entity is being tracked by the context but does not yet exist in the
		//     database.
		Added = 4,
		//
		// Summary:
		//     The entity is being tracked by the context and exists in the database, but
		//     has been marked for deletion from the database the next time SaveChanges
		//     is called.
		Deleted = 8,
		//
		// Summary:
		//     The entity is being tracked by the context and exists in the database, and
		//     some or all of its property values have been modified.
		Modified = 16,
	}
}
