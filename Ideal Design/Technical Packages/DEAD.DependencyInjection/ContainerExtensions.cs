
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DependencyInjection
{
	public static class ContainerExtensions
	{
		public static IIoCContainer RegisterType<T>(this IIoCContainer container)
		{

			return container.RegisterType(typeof(T));
		}

		public static IIoCContainer RegisterType<TFrom, TTo>(this IIoCContainer container) where TTo : TFrom
		{

			return container.RegisterType(typeof(TFrom), typeof(TTo));
		}

		public static IIoCContainer RegisterType<TFrom, TTo>(this IIoCContainer container, string name) where TTo : TFrom
		{
			return container.RegisterType(typeof(TFrom), typeof(TTo), name);
		}


		public static IIoCContainer RegisterType<T>(this IIoCContainer container, string name)
		{	
			return container.RegisterType(null, typeof(T), name);
		}	

		public static IIoCContainer RegisterType(this IIoCContainer container, Type from, Type to)
		{	
			return container.RegisterType(from, to, null);
		}	

		public static IIoCContainer RegisterType(this IIoCContainer container, Type t)
		{

			return container.RegisterType(null, t, null);
		}

		public static IIoCContainer RegisterType(this IIoCContainer container, Type t, string name)
		{
			return container.RegisterType(null, t, name);
		}


		public static IIoCContainer RegisterInstance<TInterface>(this IIoCContainer container, TInterface instance)
		{
			return container.RegisterInstance(typeof(TInterface), null, instance);
		} 

		public static IIoCContainer RegisterInstance<TInterface>(this IIoCContainer container, string name, TInterface instance)
		{
			return container.RegisterInstance(typeof(TInterface), name, instance);
		}

		public static IIoCContainer RegisterInstance(this IIoCContainer container, Type t, object instance)
		{

			return container.RegisterInstance(t, null, instance);
		}
			 
		public static T Resolve<T>(this IIoCContainer container)
		{		  
			return (T)((object)container.Resolve(typeof(T), null));
		}


		public static T Resolve<T>(this IIoCContainer container, string name)
		{					 
			return (T)((object)container.Resolve(typeof(T), name));
		}

		public static object Resolve(this IIoCContainer container, Type t)
		{
			return container.Resolve(t, null);
		}

	}

}
