using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DEAD.DependencyInjection;
using DEAD.DomainPatterns;
using System.Threading.Tasks;
using SampleMyBusinessSolution1.Models;
using System.Data.Entity;

namespace SampleMyBusinessSolution1.Services.Test
{
	[TestClass]
	public class Connectability
	{
		static Connectability()
		{

			HostConfigures.HostProfiles.TestingConfigure.Configure(IoCManager.Instance);

		}

		[TestMethod]
		public async Task Connect()
		{
			using (var uow = IoCManager.Instance.DefualtContainer.Resolve<IUnitOfWork>())
			{
				Assert.IsNotNull(uow, "UOW解析失败");
				Assert.IsNotNull(uow.GetUOWCore(), "UOW内部的DbContext解析失败");
				var u = new User() { Name = "Test" };
				var repo = uow.GetRepository<User>();
				var count = await repo.Entities.CountAsync();
				repo.Add(u);
				await uow.SubmitAsync();
				Assert.AreNotEqual(0, u.Id);
				repo.Remove(u);
				await uow.SubmitAsync();
				Assert.AreEqual(await repo.Entities.CountAsync(), count);
			}
		}
	}
}
