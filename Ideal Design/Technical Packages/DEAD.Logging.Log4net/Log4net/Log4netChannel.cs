using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging.Log4net
{
    public abstract class Log4netChannelBase<TLevel> : DiscretedChannelBase<TLevel> where TLevel : struct
    {
        public Log4netChannelBase(string name, ILog log4netLogger) : base(name, -1)
        {
            _l4nLogger = log4netLogger;
        }

        protected ILog _l4nLogger;


        public override void Flush()
        {

        }

        public override async Task FlushAsync()
        {
            await Task.Yield();
        }


    }
}
