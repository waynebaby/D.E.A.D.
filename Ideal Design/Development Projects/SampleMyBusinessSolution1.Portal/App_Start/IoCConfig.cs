using DEAD.DependencyInjection;
using DEAD.DependencyInjection.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SampleMyBusinessSolution1.Portal
{
	public class IoCConfig
	{
		public static void ConfigureIoC()
		{
			HostConfigures.HostProfiles.WebSiteConfigure.Configure(IoCManager.Instance);
		}


	}
}