using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{
	public interface ILoggers<TLevel>
	{
		IChannel<TLevel> this[TLevel level] { get; }	  

	}
}
