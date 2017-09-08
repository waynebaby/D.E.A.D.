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
	public class MirgrationDisabledMappedDbContext<T> : MappedDbContext<T>
	{

		public MirgrationDisabledMappedDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{


		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Database.SetInitializer<MirgrationDisabledMappedDbContext<T>>(new MirgrationDisabledCreateDatabaseIfNotExists<MirgrationDisabledMappedDbContext<T>>());
			base.OnModelCreating(modelBuilder);
		}

	
	}


	internal class MirgrationDisabledCreateDatabaseIfNotExists<TContext> : CreateDatabaseIfNotExists<TContext> where TContext : DbContext
	{
		public override void InitializeDatabase(TContext context)
		{
			base.InitializeDatabase(context);
			context.Database.ExecuteSqlCommand("IF (SELECT OBJECT_ID('dbo.__MigrationHistory')) IS NOT NULL DROP TABLE  [dbo].[__MigrationHistory];");
		}
	}

}
