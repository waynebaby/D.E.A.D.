using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEAD.Caching
{
	public interface ICacheGroup
	{

		void ForceExpireAll();

		ICacheDictionary this[string name]
		{
			get;
		}

	}
}
