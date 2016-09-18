using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace DEAD.Logging.Log4net
{
    public class Log4netStandardChannel : Log4netChannelBase<StandardLevels>, IChannel<StandardLevels>
    {
        public Log4netStandardChannel(string name, StandardLevels level, ILog log4netLogger) : base(name, log4netLogger)
        {
            Level = level;
        }


        protected override async Task OnWriteMessageAsync(LoggingMessage message)
        {
            var msg = message.ToString();
            switch (Level)
            {
                case StandardLevels.Critical:
                    _l4nLogger.Fatal(msg);
                    break;

                case StandardLevels.Important:
                    _l4nLogger.Error(msg);

                    break;
                case StandardLevels.Warning:
                    _l4nLogger.Warn(msg);

                    break;
                case StandardLevels.Infomation:
                    _l4nLogger.InfoFormat(msg);

                    break;
                case StandardLevels.Debug:
                    _l4nLogger.Debug(msg);

                    break;
                default:
                    break;
            }
            await Task.Yield();
        }
    }
}
