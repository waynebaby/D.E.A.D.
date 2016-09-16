using DEAD.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{
    public abstract class LoggerBase<TLevel> : ILogger<TLevel>, IIoCContexted where TLevel : struct
    {
        static protected Dictionary<TLevel, string> _NameCache
              = new Dictionary<TLevel, string>();

        public virtual IChannel<TLevel> this[TLevel level]
        {
            get
            {
                string levelString = null;
                if (!_NameCache.TryGetValue(level, out levelString))
                {
                    levelString = level.ToString();
                    lock (_NameCache)
                    {
                        _NameCache[level] = levelString;
                    }
                }
                return this.GetIoCManager().DefualtContainer.Resolve<IChannel<TLevel>>(levelString);
            }
        }

        public IIoCContext IoCContext
        {
            get; set;
        }
    }

    public class StandardLogger : LoggerBase<StandardLevels>, IStandardLogger
    {
        public IChannel<StandardLevels> Critical
        {
            get
            {
                return this[StandardLevels.Critical];
            }
        }

        public IChannel<StandardLevels> Debug
        {
            get
            {
                return this[StandardLevels.Debug];

            }
        }

        public IChannel<StandardLevels> Important
        {
            get
            {
                return this[StandardLevels.Important];

            }
        }

        public IChannel<StandardLevels> Infomation
        {
            get
            {
                return this[StandardLevels.Infomation];

            }
        }

        public IChannel<StandardLevels> Warning
        {
            get
            {
                return this[StandardLevels.Warning];

            }
        }
    }
}
