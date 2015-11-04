using SampleMyBusinessSolution1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMyBusinessSolution1.Contracts
{
	public interface IGroupService
	{
		Task<Group> AddGroupAsync(Group Group);

		Task<Group> UpdateGroupAsync(Group Group);



		Task RemoveGroupAsync(Group Group);
	}
}
