using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Concurrent;

namespace DEAD.DependencyInjection.Implementations
{
    public class AutofacContainer : IIoCContainer
    {
        Autofac.ContainerBuilder builder;
        Autofac.IContainer container;
        public AutofacContainer()
        {
            builder = new Autofac.ContainerBuilder();

        }
        public IIoCContext Context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object GetContainerCore(Type containerCoreType)
        {
            switch (containerCoreType)
            {
                case var m when m == typeof(IContainer):
                    return container;

                case var m when m == typeof(ContainerBuilder):
                    return builder;
                default:
                    return null;
            }
        }

        public TContainerCore GetContainerCore<TContainerCore>() where TContainerCore : class
        {
            return (TContainerCore)GetContainerCore(typeof(TContainerCore));
        }

        public IIoCContainer RegisterInstance<TTo>(string name, object instance) where TTo : class
        {
            CheckBuilt();
            builder.RegisterInstance((TTo)instance).Named<TTo>(name);
            return this;
        }



        public IIoCContainer RegisterInstance(Type t, string name, object instance)
        {
            Func<object, string, object, IIoCContainer> g = GetRegisterInstanceAccessor(t);

            return g.Invoke(this, name, instance);

        }

        ConcurrentDictionary<Type, Lazy<Func<object, string, object, IIoCContainer>>> cache =
            new ConcurrentDictionary<Type, Lazy<Func<object, string, object, IIoCContainer>>>();

        private Func<object, string, object, IIoCContainer> GetRegisterInstanceAccessor(Type t)
        {
            Func<object, string, object, IIoCContainer> rval = 
                cache
                    .GetOrAdd(
                        t, 
                        new Lazy<Func<object, string, object, IIoCContainer>>(() =>
                        {

                            var mi = GetType().GetTypeInfo()
                                    .GetMethods()
                                    .Where(x => x.Name == nameof(RegisterInstance) && x.IsGenericMethod)
                                    .Single();
                            var gi = mi.MakeGenericMethod(t);

                            var d = gi.CreateDelegate(typeof(Func<object, string, object, IIoCContainer>));
                            return d as Func<object, string, object, IIoCContainer>;

                        },true))
                    .Value;



            return rval;
        }

        private void CheckBuilt()
        {
            if (Status == AutofacContainerStatus.Built)
            {
                throw new InvalidOperationException("You Cannot register after first resolve");
            }
        }

        public IIoCContainer RegisterType(Type from, Type to, string name)
        {
            CheckBuilt();
            builder.RegisterType(to).Named(name, from);
            return this;
        }

        public object Resolve(Type t, string name)
        {
            BuildIfNotBuilt();
            return container.ResolveNamed(name, t);
        }

        void BuildIfNotBuilt()
        {
            lock (this)
            {
                if (Status == AutofacContainerStatus.Configuring)
                {
                    container = builder.Build();
                    Status = AutofacContainerStatus.Built;
                }
            }
        }



        public AutofacContainerStatus Status { get; set; }
        public ContainerBuilder AutoFacBuilder { get => builder; set => builder = value; }
        public IContainer AutoFacContainer { get => container; set => container = value; }
    }

    public enum AutofacContainerStatus
    {
        Configuring,
        Built
    }
}
