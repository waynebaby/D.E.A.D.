using SampleMyBusinessSolution1.HostConfigures.Configures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMyBusinessSolution1.HostConfigures
{
	public static class HostProfiles
	{
		public static ServiceHostConfigure ServiceHostConfigure { get; set; }

		public static WebSiteConfigure WebSiteConfigure { get; set; }


		static TestingConfigure _TestingConfigure = new TestingConfigure();
		public static TestingConfigure TestingConfigure
		{
			get
			{
				return _TestingConfigure;
			}

			set
			{
				_TestingConfigure = value;
			}
		}
	}
}
