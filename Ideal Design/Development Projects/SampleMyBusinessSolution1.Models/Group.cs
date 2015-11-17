using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleMyBusinessSolution1.Models
{
	public class Group
	{

		public int Id { get; set; }		 
		public string Name { get; set; }
		public ICollection<User> Users { get; set; }

	}
}
