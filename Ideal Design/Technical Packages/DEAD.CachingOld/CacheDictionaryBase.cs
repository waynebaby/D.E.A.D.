using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Caching
{
	public abstract class CacheDictionaryBase : ICacheDictionary
	{
		protected CacheDictionaryBase(string name, ICacheGroup group)
		{
			Name = name;
			CurrentGroup = group;
		}


		public virtual string Name
		{
			get; protected set;
		}

		public virtual Guid CurrentVersion
		{
			get;
			protected set;
		}
		public virtual ICacheGroup CurrentGroup
		{
			get; protected set;
		}

		KeyValuePair<Guid, KeyValuePair<Guid, string>> Prefix;
		protected virtual string GetKeyPrefix()
		{
			var vg = CurrentGroup.CurrentVersion;
			var vd = CurrentVersion;
			var kcopy = Prefix;
			if (kcopy.Key != vg || kcopy.Value.Key == vd)
			{
				kcopy = new KeyValuePair<Guid, KeyValuePair<Guid, string>>(vg, new KeyValuePair<Guid, string>(vd, string.Format("_{0}_{1}_", vg, vd)));
				Prefix = kcopy;
			}
			return kcopy.Value.Value;

		}


		public virtual void ForceExpireAll()
		{
			CurrentVersion = Guid.NewGuid();
		}








		protected abstract object OnAddOrGetExisting(string prefixedKey, object value, DateTimeOffset absoluteExpiration);
		protected abstract object OnAddOrGetExisting(string prefixedKey, object value, CacheItemPriority priority = CacheItemPriority.Default);
		protected abstract object OnGet(string prefixedKey);
		protected abstract IDictionary<string, object> OnGetValues(IEnumerable<string> prefixedKeys);
		protected abstract object OnRemove(string prefixedKey);
		protected abstract void OnSet(string prefixedKey, string value, TimeSpan slidingExpiration);
		protected abstract void OnSet(string prefixedKey, string value, DateTimeOffset absoluteExpiration);
		protected abstract void OnSet(string prefixedKey, object value, CacheItemPriority priority = CacheItemPriority.Default);

		protected abstract bool OnContains(string prefixedKey);

		public bool Contains(string key)
		{
			return OnContains(GetKeyPrefix() + key);
		}
		public object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration)
		{
			return OnAddOrGetExisting(GetKeyPrefix() + key, value, absoluteExpiration);
		}
		public object AddOrGetExisting(string key, object value, CacheItemPriority priority = CacheItemPriority.Default)
		{
			return OnAddOrGetExisting(GetKeyPrefix() + key, value, priority);

		}
		public object Get(string key)
		{
			return OnGet(GetKeyPrefix() + key);
		}
		public IDictionary<string, object> GetValues(IEnumerable<string> keys)
		{
			return OnGetValues(keys.Select(x => GetKeyPrefix() + x));
		}
		public object Remove(string key)
		{
			return OnRemove(GetKeyPrefix() + key);
		}
		public void Set(string key, string value, DateTimeOffset absoluteExpiration)
		{
			OnSet(GetKeyPrefix() + key, value, absoluteExpiration);
		}
		public void Set(string key, object value, CacheItemPriority priority = CacheItemPriority.Default)
		{

			OnSet(GetKeyPrefix() + key, value, priority);
		}
		public void Set(string key, string value, TimeSpan slidingExpiration)
		{
			OnSet(GetKeyPrefix() + key, value, slidingExpiration);
		}
													

		public abstract long GetCount();
		public abstract void Dispose();
	}
}
