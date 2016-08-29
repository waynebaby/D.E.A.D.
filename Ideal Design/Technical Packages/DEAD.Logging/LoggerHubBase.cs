using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{
	public abstract class LoggerHubBase<TLevel> : ILoggerHub<TLevel>
	{
		public ILogger this[TLevel level]
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
