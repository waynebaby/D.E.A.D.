using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DependencyInjection.Implementations
{
	public class IoCContext : IIoCContext
	{

		public IoCContext(IIoCManager manager,object contextBag)
		{
			Manager = manager;
			ContextBag = contextBag;
		}
		public dynamic ContextBag { get; private set; }

		public IIoCManager Manager
		{
			get;  private set;
		
		}
	}
}
