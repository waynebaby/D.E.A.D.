using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DependencyInjection
{
	public interface IIoCConfigure
	{
		void Configure(IIoCManager manager);
	}
}
