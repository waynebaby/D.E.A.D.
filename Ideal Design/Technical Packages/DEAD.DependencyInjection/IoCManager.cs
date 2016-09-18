using DEAD.DependencyInjection;
using DEAD.DependencyInjection.Implementations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DependencyInjection
{
    public class IoCManager : IIoCManager
    {
        static IoCManager()
        {
            Instance = new IoCManager();
        }
        public IoCManager()
        {
            core = new ConcurrentDictionary<string, IIoCContainer>();
            Containers = new ReadOnlyDictionary<string, IIoCContainer>(core);

        }
        ConcurrentDictionary<string, IIoCContainer> core;
        public IDictionary<string, IIoCContainer> Containers { get; private set; }


        public IIoCContainer DefualtContainer
        {
            get
            {
                var key = string.Empty;
                IIoCContainer rval;

                if (!Containers.TryGetValue(key, out rval))
                {
                    rval = new UnityIoCContainer(new IoCContext(this, new ExpandoObject()));
                    ConfigureDefaultContianer(() => rval);
                }
                return rval;
            }
        }

        public void ConfigureContianer(string name, Func<IIoCContainer> factory)
        {
            var v = factory();
            v.Context = v.Context ?? new IoCContext(this, ContextBag);
            core.AddOrUpdate(name, v, (_1, _2) => v);
        }

        public void ConfigureDefaultContianer(Func<IIoCContainer> factory)
        {
            ConfigureContianer(string.Empty, factory);
        }


        public static IoCManager Instance { get; set; }

        public string Name
        {
            get; set;
        }

        public dynamic ContextBag { get; } = new ExpandoObject();
    }
}
