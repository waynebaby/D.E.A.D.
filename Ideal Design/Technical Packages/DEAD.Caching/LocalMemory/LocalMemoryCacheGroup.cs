using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Caching.LocalMemory
{
	public class LocalMemoryCacheGroup : ICacheGroup
	{
		public ICacheDictionary this[string name]
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public Guid CurrentVersion
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public void ForceExpireAll()
		{
			throw new NotImplementedException();
		}

		public Tuple<Guid, IDictionary<string, ICacheDictionary>> GetCurrentDirectionarys()
		{
			throw new NotImplementedException();
		}

		public void Update(ICacheDictionary newVersionOfCacheDictionary)
		{
			throw new NotImplementedException();
		}

		public void Update(ICacheGroup newVersionOfCacheGroup)
		{
			throw new NotImplementedException();
		}
	}
}
