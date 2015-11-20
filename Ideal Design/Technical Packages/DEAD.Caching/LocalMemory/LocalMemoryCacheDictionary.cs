using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DEAD.Caching.LocalMemory
{

	public class LocalMemoryCacheDictionary : ObjectCacheBasedCacheDictionaryBase, IDisposable
	{
		protected LocalMemoryCacheDictionary(string name, ICacheGroup group) : base(name, group, new MemoryCache(name))
		{
		}

		#region IDisposable Support


		private int disposedValue = 0; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (Interlocked.Exchange(ref disposedValue, 1) == 0)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					var dtar = (_core as IDisposable);
					if (dtar != null)
					{
						dtar.Dispose();
					}

				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~LocalMemoryCacheDictionary() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.


		public override void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);

		}
		#endregion


	}
}
