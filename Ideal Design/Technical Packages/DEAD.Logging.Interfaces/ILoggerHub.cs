using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{
	public interface ILoggerHub<TLevel>
	{
		ILogger this[TLevel level] { get; }		   

	}
}
