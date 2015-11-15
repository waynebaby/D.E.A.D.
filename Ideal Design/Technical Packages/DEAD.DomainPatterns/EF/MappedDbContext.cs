﻿using DEAD.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DomainPatterns.EF
{
	public class MappedDbContext<T> : DbContext
	{
		public MappedDbContext(string nameOrConnectionString)
		   : base(nameOrConnectionString)
		{


		}

	


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Assembly loadingAssembly = IoCManager.Instance.DefualtContainer.Resolve<Assembly>("For"+this.GetType().FullName);
			modelBuilder.Configurations.AddFromAssembly(loadingAssembly);
            base.OnModelCreating(modelBuilder);
		}


	}
}