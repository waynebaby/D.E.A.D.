using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{
	public interface ILogger
	{
		string Name { get; }		

		void Log(string message, [CallerMemberName]string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null);
		void Log(Action<StringBuilder> buildAction, [CallerMemberName]string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null);

		void Flush();

		Task FlushAsync();

		long QueueSize { get; }



	}
}
