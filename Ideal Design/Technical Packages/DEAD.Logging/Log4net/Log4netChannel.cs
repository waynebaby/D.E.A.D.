using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging.Log4Net
{
    public class Log4NetChannel<TLevel> : ChannelBase<TLevel>
    {
        public override TLevel Level
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long QueueSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override Task FlushAsync()
        {
            throw new NotImplementedException();
        }

        public override void Log(string message, [CallerMemberName] string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null)
        {
            throw new NotImplementedException();
        }

        public override void Log(Action<StringBuilder> buildAction, [CallerMemberName] string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null)
        {
            throw new NotImplementedException();
        }
    }
}
