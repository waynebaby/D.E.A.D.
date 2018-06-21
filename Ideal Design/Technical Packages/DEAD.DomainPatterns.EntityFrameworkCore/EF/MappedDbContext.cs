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
    public class MappedDbContext<T> : DbContext, IIoCContexted
    {
        public MappedDbContext(string nameOrConnectionString)
            : base()
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            Assembly loadingAssembly = this.GetIoCManager().DefualtContainer.Resolve<Assembly>(RegisterModelConfiguresName);

            var configurations = loadingAssembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(
                    it =>
                        it.IsInterface &&
                        it.IsGenericType &&
                        it.GenericTypeArguments.Length == 1 &&
                        (it.GetConstructor(Array.Empty<Type>())?.IsPublic ?? false) &&
                        it.IsAssignableFrom(typeof(IEntityTypeConfiguration<>)
                            .MakeGenericType(it.GenericTypeArguments[0]))));

            foreach (var config in configurations)
            {
                dynamic instance = System.Activator.CreateInstance(config);
                modelBuilder.ApplyConfiguration(instance);
            }
            base.OnModelCreating(modelBuilder);
        }



        public static string RegisterModelConfiguresName => "For" + typeof(MappedDbContext<T>).FullName;


        public IIoCContext IoCContext
        {
            get; set;
        }



    }
}
