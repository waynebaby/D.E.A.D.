using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEAD.Caching
{
	public enum CacheItemPriority
	{
		// Summary:
		//     Indicates that there is no priority for removing the cache entry.
		Default = 0,
		//
		// Summary:
		//     Indicates that a cache entry should never be removed from the cache.
		NotRemovable = 1,
	}


	public interface ICacheDictionary : IDisposable
	{
		void ForceExpireAll();
		string Name { get; }


		object AddOrGetExisting(string key, object value, CacheItemPriority priority = CacheItemPriority.Default);
		object AddOrGetExisting(string key, object value, DateTimeOffset absoluteExpiration);

		bool Contains(string key);


		object Get(string key);

		//public override CacheItem GetCacheItem(string key, string regionName = null);

		long GetCount(string regionName = null);

		IDictionary<string, object> GetValues(IEnumerable<string> keys);

		object Remove(string key);

		void Set(string key, object value, CacheItemPriority priority = CacheItemPriority.Default);

		void Set(string key, string value, DateTimeOffset absoluteExpiration);
		void Set(string key, string value, System.TimeSpan slidingExpiration);




	}
}
