using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DependencyInjection
{
	public interface IIoCContainer
	{

		IIoCContainer RegisterInstance(System.Type t, string name, object instance);
		IIoCContainer RegisterType(System.Type from, System.Type to, string name);

		object GetContainerCore(Type containerCoreType);	 
		TContainerCore GetContainerCore<TContainerCore>() where TContainerCore:class;

		Object Resolve(Type t, string name);

		IIoCContext Context { get; set; }

	}
}
