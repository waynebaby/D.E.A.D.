using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.Logging
{
	public abstract class LoggerHubBase<TLevel> : ILoggers<TLevel>
	{
		public LoggerHubBase(IDictionary<TLevel, ILogger<TLevel>> loggers)
			: this(loggers.ToDictionary(x => x.Key, x => new Lazy<ILogger<TLevel>>(() => x.Value, false)))
		{

		}

		public LoggerHubBase(IDictionary<TLevel, Func<ILogger<TLevel>>> loggers)
			: this(loggers.ToDictionary(x => x.Key, x => new Lazy<ILogger<TLevel>>(x.Value, true)))
		{
		}


		public LoggerHubBase(IDictionary<TLevel, Lazy<ILogger<TLevel>>> loggers)
		{
			_loggers = new ConcurrentDictionary<TLevel, Lazy<ILogger<TLevel>>>(loggers);
		}

		protected ConcurrentDictionary<TLevel, Lazy<ILogger<TLevel>>> _loggers;

		IChannel<TLevel> ILogger<TLevel>.this[TLevel level]
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public virtual ILogger<TLevel> this[TLevel level] { get { return _loggers[level].Value; } }

	}
}
