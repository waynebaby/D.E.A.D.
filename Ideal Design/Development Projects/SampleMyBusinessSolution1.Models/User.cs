﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMyBusinessSolution1.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? GroupId { get; set; }
		public Group Group { get; set; }

	}
}