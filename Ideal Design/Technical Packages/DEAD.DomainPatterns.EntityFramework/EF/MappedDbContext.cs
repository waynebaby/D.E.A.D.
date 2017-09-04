using DEAD.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.DomainPatterns.EF
{
	public class MappedDbContext<T> : DbContext, IIoCContexted
	{
		public MappedDbContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{


		}																   

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			
			Assembly loadingAssembly = this.GetIoCManager().DefualtContainer.Resolve<Assembly>(RegisterModelConfiguresName);
			modelBuilder.Configurations.AddFromAssembly(loadingAssembly);
			base.OnModelCreating(modelBuilder);
		}

		static readonly string _RegisterModelConfiguresName = "For" + typeof(MappedDbContext<T>).FullName;
		public static string RegisterModelConfiguresName
		{
			get { return _RegisterModelConfiguresName; }
		}

		public IIoCContext IoCContext
		{
			get; set;
		}
	}
}
