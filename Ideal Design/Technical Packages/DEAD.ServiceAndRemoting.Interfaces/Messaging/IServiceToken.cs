using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DEAD.ServiceAndRemoting.Messaging
{

	/// <summary>
	/// for wcf or other service which need adiitional disposing actions.
	/// </summary>
	/// <typeparam name="TContract"></typeparam>
	public interface IServiceToken<TContract> : IServiceToken, IDisposable
	{
		TContract Service { get; }
	}


	public interface IServiceToken : IDisposable
	{
		Type ServiceType { get; }

		Object ServiceObject { get; }

	}
}
