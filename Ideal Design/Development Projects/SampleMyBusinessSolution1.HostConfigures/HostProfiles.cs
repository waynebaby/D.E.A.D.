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
		static ServiceHostConfigure _serviceHostConfigure = new ServiceHostConfigure();

		static WebSiteConfigure _webSiteConfigure = new WebSiteConfigure();


		static TestingConfigure _testingConfigure = new TestingConfigure();
		public static TestingConfigure TestingConfigure
		{
			get
			{
				return _testingConfigure;
			}

			set
			{
				_testingConfigure = value;
			}
		}

		public static ServiceHostConfigure ServiceHostConfigure
		{
			get
			{
				return _serviceHostConfigure;
			}

			set
			{
				_serviceHostConfigure = value;
			}
		}

		public static WebSiteConfigure WebSiteConfigure
		{
			get
			{
				return _webSiteConfigure;
			}

			set
			{
				_webSiteConfigure = value;
			}
		}
	}
}
