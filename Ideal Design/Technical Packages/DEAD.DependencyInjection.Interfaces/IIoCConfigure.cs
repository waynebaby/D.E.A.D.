using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEAD.DependencyInjection
{
	public interface IIoCConfigure
	{
		void Configure(IIoCManager manager);
	}
}
