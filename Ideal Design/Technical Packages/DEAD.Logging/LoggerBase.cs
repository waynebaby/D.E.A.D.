using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{
	public abstract class LoggerBase : ILogger
	{
		public abstract string Name { get; }
		public abstract long QueueSize { get; }

		public abstract void Flush();
		public abstract Task FlushAsync();
		public abstract void Log(Action<StringBuilder> buildAction, [CallerMemberName] string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null);
		public abstract void Log(string message, [CallerMemberName] string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null);
	}
}
