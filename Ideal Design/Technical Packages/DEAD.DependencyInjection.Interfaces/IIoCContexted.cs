using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEAD.DependencyInjection
{
	public interface IIoCContexted
	{											
		IIoCContext IoCContext { get; set; }
	}
}
