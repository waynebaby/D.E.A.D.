using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{
	public interface ILogger<TLevel> where TLevel: struct
	{
		IChannel<TLevel> this[TLevel level] { get; }	  

	}
}
