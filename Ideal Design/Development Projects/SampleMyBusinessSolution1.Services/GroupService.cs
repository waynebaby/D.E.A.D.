﻿using AutoMapper;

using SampleMyBusinessSolution1.Contracts;
using SampleMyBusinessSolution1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
namespace SampleMyBusinessSolution1.Services
{
	public class GroupService : IGroupService
	{
		public async Task<Group> AddGroupAsync(Group group)
		{
			using (var uow = IoCManager.Instance.DefualtContainer.Resolve<IUnitOfWork>())
			{
				var repo = uow.GetRepository<Group>();
				repo.Add(group);
				await uow.SubmitAsync();
			}
			return group;
		}

		public async Task<Group> GetGroupAsync(int groupId)
		{
			using (var uow = IoCManager.Instance.DefualtContainer.Resolve<IUnitOfWork>())
			{
				var repo = uow.GetRepository<Group>();
				var target = await repo
				.Entities
				.Where(x => x.Id == groupId)
				.SingleAsync();
				return target;
			}
		}

		public async Task RemoveGroupAsync(Group group)
		{
			using (var uow = IoCManager.Instance.DefualtContainer.Resolve<IUnitOfWork>())
			{
				var repo = uow.GetRepository<Group>();
				repo.AttachAndRefresh(group);
				repo.Remove(group);
				await uow.SubmitAsync();
			}

		}

		public async Task<Group> UpdateGroupAsync(Group group)
		{
			using (var uow = IoCManager.Instance.DefualtContainer.Resolve<IUnitOfWork>())
			{
				var repo = uow.GetRepository<Group>();
				var target = await repo
					.Entities
					.Where(x => x.Id == group.Id)
					.SingleAsync();
				Mapper.DynamicMap(group, target);
				await uow.SubmitAsync();
				return target;
			}

		}



	}
}
