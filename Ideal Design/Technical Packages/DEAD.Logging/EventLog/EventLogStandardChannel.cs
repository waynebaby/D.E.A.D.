using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging.EventLog
{
    public class EventLogStandardChannel : EventLogChannelBase, IChannel<StandardLevels>
    {
        public EventLogStandardChannel(string name, StandardLevels level) : base(name)
        {
            Level = level;
        }

        public StandardLevels Level
        {
            get; protected set;
        }

        protected override EventLogEntryType EventLogEntryType
        {
            get
            {
                switch (Level)
                {
                    case StandardLevels.Critical:
                        return EventLogEntryType.FailureAudit;
                    case StandardLevels.Important:
                        return EventLogEntryType.Error;
                    case StandardLevels.Warning:
                        return EventLogEntryType.Warning;
                    case StandardLevels.Infomation:
                        return EventLogEntryType.Information;
                    case StandardLevels.Debug:
                        return EventLogEntryType.SuccessAudit;
                    default:
                        return EventLogEntryType.Information;
                }
            }
        }
    }
}
