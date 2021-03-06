﻿using SampleMyBusinessSolution1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMyBusinessSolution1.Contracts
{
	public interface IGroupService
	{
		Task<Group> AddGroupAsync(Group group);

		Task<Group> UpdateGroupAsync(Group group);
		Task<Group> GetGroupAsync(int groupId);

		Task RemoveGroupAsync(Group group);
	}
}
