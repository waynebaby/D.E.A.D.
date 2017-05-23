using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{
	public interface IChannel
	{
		string Name { get; }

		void Log(string message,string detailedInfomation, [CallerMemberName]string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null);
		void Log(string message,ExpandoObject detailedInfomation, [CallerMemberName]string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null);
		void Log(string message,  Action<StringBuilder> detailedInfomationBuilder, [CallerMemberName]string member = null, [CallerLineNumber] int line = -1, [CallerFilePath] string filePath = null);
		void Flush();
		Task FlushAsync();
		long QueueSize { get; }



	}

	public interface IChannel<TLevel> : IChannel where TLevel : struct
    {
		TLevel Level { get; }

	}
}
