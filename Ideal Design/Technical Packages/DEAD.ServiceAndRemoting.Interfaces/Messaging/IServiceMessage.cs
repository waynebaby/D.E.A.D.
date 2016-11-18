using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.Messaging
{

	/// <summary>
	/// 服务消息
	/// </summary>
	public interface IServiceMessage : IPeekLockToken
	{

		/// <summary>Gets the body.</summary>
		/// <value>The body.</value>
		object Body { get; }

		/// <summary>
		/// 服务消息的附加属性
		/// </summary>
		IDictionary<string, object> Properties { get; }
		/// <summary>
		/// 服务消息属性快速访问通道
		/// </summary>
		dynamic PropertyBags { get; }


		///// <summary>Gets the common headers.</summary>
		///// <value>The common headers.</value>
		//IMessageHeader CommonHeaders { get; }

		/// <summary>
		/// 服务消息体
		/// </summary>
		T GetBody<T>();

		/// <summary>
		/// 获取服务消息体类型
		/// </summary>
		/// <returns></returns>
		Type GetBodyType();

		/// <summary>
		/// 获取服务消息体类型
		/// </summary>
		/// <returns></returns>
		object GetBody(Type type);
	}
    

}
