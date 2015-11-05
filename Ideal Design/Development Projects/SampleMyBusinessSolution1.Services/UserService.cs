using SampleMyBusinessSolution1.Contracts;
using SampleMyBusinessSolution1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMyBusinessSolution1.Services
{
	public class UserService : IUserService
	{
		public Task<User> AddUserAsync(User user)
		{
			throw new NotImplementedException();

		}

		public Task<User> MoveUserToGroupAsync(Group group)
		{
			throw new NotImplementedException();
		}

		public Task<User> UpdateUserAsync(User user)
		{
			throw new NotImplementedException();
		}
	}
}
