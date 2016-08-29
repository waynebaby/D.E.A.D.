using System;
using System.Collections.Generic;

namespace DEAD.DependencyInjection
{
	public interface IIoCManager
	{

		string Name { get; set; }
		void ConfigureContianer(string name, Func<IIoCContainer> factory);
		void ConfigureDefaultContianer(Func<IIoCContainer> factory);
		IIoCContainer DefualtContainer { get; }
		IDictionary<string, IIoCContainer> Containers { get; }

		dynamic ContextBag { get;}
	}
}