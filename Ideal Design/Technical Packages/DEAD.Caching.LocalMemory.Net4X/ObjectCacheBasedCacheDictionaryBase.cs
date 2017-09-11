using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Caching
{
	public abstract class ObjectCacheBasedCacheDictionaryBase : CacheDictionaryBase
	{

		protected ObjectCacheBasedCacheDictionaryBase(string name, ICacheGroup group, ObjectCache cacheCore) : base(name, group)
		{

			_core = cacheCore;
		}

		protected ObjectCache _core;

		protected override object OnAddOrGetExisting(string prefixedKey, object value, DateTimeOffset absoluteExpiration)
		{

			var c = new CacheItem(prefixedKey, value);
			var p = new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration, Priority = System.Runtime.Caching.CacheItemPriority.Default };
			var o = _core.AddOrGetExisting(c, p);
			return o.Value;

		}

		protected override object OnAddOrGetExisting(string prefixedKey, object value, CacheItemPriority priority = CacheItemPriority.Default)
		{

			var prior = System.Runtime.Caching.CacheItemPriority.Default;

			if (priority == CacheItemPriority.NotRemovable)
			{
				priority = CacheItemPriority.NotRemovable;
			}
			var c = new CacheItem(prefixedKey, value);
			var p = new CacheItemPolicy { Priority = prior };
			var o = _core.AddOrGetExisting(c, p);
			return o.Value;
		}

		protected override bool OnContains(string key)
		{
			return _core.Contains(key);
		}

		protected override object OnGet(string key)
		{
			return _core.Get(key);
		}
		public override long GetCount()
		{
			return _core.GetCount();
		}


		protected override IDictionary<string, object> OnGetValues(IEnumerable<string> prefixedKeys)
		{
			throw new NotImplementedException();
		}

		protected override object OnRemove(string prefixedKey)
		{
			throw new NotImplementedException();
		}


		protected override void OnSet(string prefixedKey, object value, CacheItemPriority priority = CacheItemPriority.Default)
		{
			throw new NotImplementedException();
		}


		protected override void OnSet(string prefixedKey, string value, DateTimeOffset absoluteExpiration)
		{
			throw new NotImplementedException();

		}


		protected override void OnSet(string prefixedKey, string value, TimeSpan slidingExpiration)
		{
			throw new NotImplementedException();
		}



	}
}
