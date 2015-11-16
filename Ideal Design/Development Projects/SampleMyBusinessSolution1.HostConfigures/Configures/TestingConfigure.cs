﻿using DEAD.DependencyInjection;
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

namespace SampleMyBusinessSolution1.HostConfigures.Configures
{
	public class TestingConfigure : IIoCConfigure
	{

		static string _connectionString
			= string.Format(@"
				 Data Source=(localdb)\MSSQLLocalDB;Data Source=(localdb)\v11.0; Initial Catalog=BooksAPIContext-20150419115728; Integrated Security=True; MultipleActiveResultSets=True; AttachDbFilename={0}
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
					var c = new UnityIoCContainer();

					//注册一个自定义的connection string 
					c.GetContainerCore<IUnityContainer>()
						.RegisterType<MappedDbContext<object>>(//要替换其中参数所以调用了Unity底层
						 new InjectionConstructor(new InjectionParameter(typeof(string), _connectionString)));
					//默认DbContext实现注册
					c.RegisterType<DbContext, MappedDbContext<object>>();

					//默认UOW实现注册
					c.RegisterType<IUnitOfWork, EFUnitOfWork>();

					//对于这个实现，设定一个扫描程序集 运行其中所有的模型设置：
					c.RegisterInstance<System.Reflection.Assembly>(MappedDbContext<object>.RegisterModelConfiguresName, typeof(GroupConfiguration).Assembly);

					return c;
				});
		}
	}
}
