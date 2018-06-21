using DEAD.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            base.OnModelCreating(modelBuilder);
        }

    }
}
