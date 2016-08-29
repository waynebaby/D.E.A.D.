using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DependencyInjection
{
	public static class IoCContextedExtensions
	{

		public static IIoCManager GetIoCManager(this IIoCContexted source)
		{
			var rval = source?.IoCContext?.Manager;
			if (rval == null)
			{
				Debug.Print($"IIoCContexted instance or its path 'IoCContext?.Manager' is null, returning the global singleton instance \r\n\t object:{source?.GetType()?.ToString() ?? "null"} \r\t\n stack trace: {new StackTrace().ToString()}");
				rval = IoCManager.Instance;
			}

			return rval;

		}

	}

}
