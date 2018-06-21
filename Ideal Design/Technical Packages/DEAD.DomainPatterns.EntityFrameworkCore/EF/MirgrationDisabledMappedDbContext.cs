using Microsoft.EntityFrameworkCore;

namespace DEAD.DomainPatterns.EFCore
{
    public class MirgrationDisabledMappedDbContext<T> : MappedDbContext<T>
	{

		public MirgrationDisabledMappedDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
 

        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //	Database.SetInitializer<MirgrationDisabledMappedDbContext<T>>(new MirgrationDisabledCreateDatabaseIfNotExists<MirgrationDisabledMappedDbContext<T>>());
        //	base.OnModelCreating(modelBuilder);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.Database.EnsureCreated();
            base.OnModelCreating(modelBuilder);
        }

    }



}
