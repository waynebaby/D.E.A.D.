using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DependencyInjection
{
	public abstract class IoCContainerBase : IIoCContainer
	{
		public IoCContainerBase(IIoCContext context)
		{
			Context = context;
		}

		public  IIoCContext Context { get; set; }

		public abstract object GetContainerCore(Type containerCoreType);
		public abstract TContainerCore GetContainerCore<TContainerCore>() where TContainerCore : class;
		public abstract IIoCContainer RegisterInstance(Type t, string name, object instance);
		public abstract IIoCContainer RegisterType(Type from, Type to, string name);
		public abstract object Resolve(Type t, string name);
	}
}
