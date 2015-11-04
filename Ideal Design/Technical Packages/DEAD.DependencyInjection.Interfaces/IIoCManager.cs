using System;

namespace DEAD.DependencyInjection
{
	public interface IIoCManager
	{
		void ConfigureContianer(string name, Func<IIoCContainer> factory);
		void ConfigureDefaultContianer(Func<IIoCContainer> factory);
	}
}