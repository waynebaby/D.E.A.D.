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
		static UserAndGroupTests()
		{

			HostConfigures.HostProfiles.TestingConfigure.Configure(IoCManager.Instance);

		}
		[TestMethod]
		public async Task CreateAUserAndAGroup()
		{
			var u = new User()
			{
				Name = "User" + Guid.NewGuid().ToString()
			};

			var g = new Group()
			{
				Name = "Group" + Guid.NewGuid().ToString()
			};

			var userService = IoCManager.Instance.DefualtContainer.Resolve<IUserService>();
			var groupService = IoCManager.Instance.DefualtContainer.Resolve<IGroupService>();
			g.Users = g.Users ?? new HashSet<User>();
			u = await userService.AddUserAsync(u);
			g.Users.Add(u);
			g = await groupService.AddGroupAsync(g);

			var u2 = await userService.GetUserAsync(u.Id);
			Assert.AreEqual(u2.GroupId.Value, g.Id);

		}
	}
}
