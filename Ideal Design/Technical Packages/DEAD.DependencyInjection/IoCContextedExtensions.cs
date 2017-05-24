using DEAD.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DependencyInjection
{
    public static class IoCContextedExtensions
    {

        public static IIoCManager GetIoCManager(this IIoCContexted source)
        {
            var rval = source?.IoCContext?.Manager;
            if (rval == null)
            {
                rval = IoCManager.Instance;
            }

            return rval;

        }

        public static IStandardLogger ResolveLogger(this IIoCContexted source)
        {
            var logger = source.GetIoCManager().DefualtContainer.Resolve<IStandardLogger>();
            return logger;
        }
        public static ILogger<TLevel> ResolveLogger<TLevel>(this IIoCContexted source) where TLevel:struct
        {
            var logger = source.GetIoCManager().DefualtContainer.Resolve<ILogger<TLevel>>();
            return logger;
        }

    }

}
