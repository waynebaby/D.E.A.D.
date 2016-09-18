using DEAD.DependencyInjection;
using DEAD.DependencyInjection.Implementations;
using DEAD.DomainPatterns.EF;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleMyBusinessSolution1.Models.Mapping;
using DEAD.DomainPatterns;
using SampleMyBusinessSolution1.Contracts;
using SampleMyBusinessSolution1.Services;
using System.Dynamic;

namespace SampleMyBusinessSolution1.HostConfigures.Configures
{
	public class TestingConfigure : IIoCConfigure
	{

		static string _connectionString   //Initial Catalog=TestingDatabase？
			= string.Format(@"
				 Data Source=(localdb)\MSSQLLocalDB; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename={0}
				", Path.Combine(
				Path.GetDirectoryName(typeof(TestingConfigure).Assembly.Location),
				"TestingDatabase.mdf"
				));
		public void Configure(IIoCManager manager)
		{

			//设置测试环境IoC设置 返回一个新的IoC容器
		
			manager.ConfigureDefaultContianer(
				() =>
				{
					//新的IoC容器
					var c = new UnityIoCContainer(new IoCContext(manager,new ExpandoObject()));

					//注册一个自定义的connection string 
					c.GetContainerCore<IUnityContainer>()
						.RegisterType<MappedDbContext<object>>(//要替换其中参数所以调用了Unity底层
						 new InjectionConstructor(new InjectionParameter(typeof(string), _connectionString)));
				
					//对于这个实现，设定一个扫描程序集 运行其中所有的模型设置：
					c.RegisterInstance<System.Reflection.Assembly>(MappedDbContext<object>.RegisterModelConfiguresName, typeof(GroupConfiguration).Assembly);


					//默认DbContext实现注册
					c.RegisterType<DbContext, MappedDbContext<object>>();

					//默认UOW实现注册
					c.RegisterType<IUnitOfWork, EFUnitOfWork>();

			

					//注册业务组件实现
					c.RegisterType<IUserService, UserService>();
					c.RegisterType<IGroupService, GroupService>();

					return c;
				});
		}
	}
}
