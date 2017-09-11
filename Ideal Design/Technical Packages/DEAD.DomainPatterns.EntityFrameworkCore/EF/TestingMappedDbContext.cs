using DEAD.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DomainPatterns.EFCore
{
	public class TestingMappedDbContext<T> : MappedDbContext<T>
	{

		public TestingMappedDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{


		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Database.SetInitializer<TestingMappedDbContext<T>>(new DropCreateDatabaseAlways<TestingMappedDbContext<T>>());
			base.OnModelCreating(modelBuilder);
		}

	}
}
