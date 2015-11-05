using SampleMyBusinessSolution1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMyBusinessSolution1.Contracts
{
	public interface IUserService
	{
		Task<User> AddUserAsync(User user);

		Task<User> UpdateUserAsync(User user);

		Task<User> MoveUserToGroupAsync(Group group);

	}
}
