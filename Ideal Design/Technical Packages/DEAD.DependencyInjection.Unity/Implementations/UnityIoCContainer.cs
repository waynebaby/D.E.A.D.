using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DependencyInjection.Implementations
{
	public class UnityIoCContainer : IoCContainerBase
	{
	  
		public IUnityContainer UnityContainer { get; protected set; } = new UnityContainer();

		public UnityIoCContainer(IIoCContext context) : base(context)
		{

		}



		public override object GetContainerCore(Type containerCoreType)
		{
			if (typeof(IUnityContainer).IsAssignableFrom(containerCoreType))
			{
				return UnityContainer;
			}
			return null;
		}

		public override TContainerCore GetContainerCore<TContainerCore>() 
		{
			return UnityContainer as TContainerCore;
		}

		public override IIoCContainer RegisterInstance(Type t, string name, object instance)
		{
            UnityContainer.RegisterInstance(t, name, instance, new ContainerControlledLifetimeManager());
			return this;
		}

		public override IIoCContainer RegisterType(Type from, Type to, string name)
		{
			if (typeof(IIoCContexted).IsAssignableFrom(to))
			{

                UnityContainer.RegisterType(from, to, name, new InjectionProperty(nameof(expBody.IoCContext), Context));
			}
			else
			{

                UnityContainer.RegisterType(from, to, name);
			}

			return this;
		}

		IIoCContexted expBody = null;



		public override object Resolve(Type t, string name)
		{
			var rval = UnityContainer.Resolve(t, name);
			FillContextIfNeed(rval);
			return rval;
		}

		public IEnumerable<object> ResolveAll(Type t)
		{
			var rvals = UnityContainer.ResolveAll(t)
				.Select(
					x =>
					{
						FillContextIfNeed(x);
						return x;
					});

			return rvals;
		}


		private void FillContextIfNeed(object rval)
		{
			var icrval = rval as IIoCContexted;
			if (icrval!=null && icrval.IoCContext==null)
			{
				icrval.IoCContext = this.Context;
			}
		}

	}
}
