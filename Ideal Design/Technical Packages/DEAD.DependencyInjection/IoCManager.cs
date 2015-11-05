using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public IReadOnlyDictionary<string, IIoCContainer> Containers { get; private set; }

        public IIoCContainer DefualtContainer
        {
            get
            {
                return Containers[string.Empty];
            }
        }

        public void ConfigureContianer(string name, Func<IIoCContainer> factory)
        {
            var v = factory();
            core.AddOrUpdate(name, v, (_1, _2) => v);
        }

        public void ConfigureDefaultContianer(Func<IIoCContainer> factory)
        {
            ConfigureContianer(string.Empty, factory);
        }


        public static IoCManager Instance { get; set; }



    }
}
