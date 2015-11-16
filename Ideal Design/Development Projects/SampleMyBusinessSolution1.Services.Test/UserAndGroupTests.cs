using DEAD.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleMyBusinessSolution1.Contracts;
using SampleMyBusinessSolution1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMyBusinessSolution1.Services.Test
{

	[TestClass]
	public class UserAndGroupTests
	{
		[TestMethod]
		public void CreateAUser()
		{
			var u = new User()
			{
				Name = "User" + Guid.NewGuid().ToString()
			};

			IoCManager.Instance.DefualtContainer.Resolve<IUserService>();

		}
	}
}
