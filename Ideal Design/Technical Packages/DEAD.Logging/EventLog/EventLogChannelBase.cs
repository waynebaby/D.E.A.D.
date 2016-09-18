using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using el = System.Diagnostics.EventLog;
namespace DEAD.Logging.EventLog
{
    public abstract class EventLogChannelBase<TLevel> : DiscretedChannelBase<TLevel> where TLevel : struct
    {
        public EventLogChannelBase(string name) : base(name, -1)
        {
        }

        public override void Flush()
        {
            //throw new NotImplementedException();
        }

        public override async Task FlushAsync()
        {
            await Task.Yield();
        }

        protected override async Task OnWriteMessageAsync(LoggingMessage message)
        {
            var msg = message.GetOrBuildMessage();
            el.WriteEntry(Name, msg, EventLogEntryType);
            await Task.Yield();
        }

        abstract protected EventLogEntryType EventLogEntryType { get; }

    }


}
