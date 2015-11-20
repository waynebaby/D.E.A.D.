using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEAD.Caching
{
	public interface ICacheGroup
	{
		Guid CurrentVersion { get; }
		void ForceExpireAll();

		ICacheDictionary this[string name]
		{
			get;
		}

		Tuple<Guid, IDictionary<string, ICacheDictionary>> GetCurrentDirectionarys();

		void Update(ICacheGroup newVersionOfCacheGroup);

		void Update(ICacheDictionary newVersionOfCacheDictionary);

	}
}
