using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DependencyInjection.Implementations
{
	public class UnityIoCContainer : IIoCContainer
	{
		IUnityContainer _container = new UnityContainer();
		public object GetContainerCore(Type containerCoreType)
		{
			if (typeof(IUnityContainer).IsAssignableFrom(containerCoreType))
			{
				return _container;
			}
			return null;
		}

		public TContainerCore GetContainerCore<TContainerCore>() where TContainerCore : class
		{
			return _container as TContainerCore;
		}

		public IIoCContainer RegisterInstance(Type t, string name, object instance)
		{
			_container.RegisterInstance(t, name, instance, new ContainerControlledLifetimeManager());
			return this;
		}

		public IIoCContainer RegisterType(Type from, Type to, string name)
		{
			_container.RegisterType(from, to, name);
			return this;
		}

		public object Resolve(Type t, string name)
		{
			return _container.Resolve(t, name);
		}


		public IEnumerable<object> ResolveAll(Type t)
		{
			return _container.ResolveAll(t);
		}
	}
}
