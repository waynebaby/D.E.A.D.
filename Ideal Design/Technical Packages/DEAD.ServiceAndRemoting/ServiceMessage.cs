using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.Messaging
{




	/// <summary>
	/// 消息头（Header）：定义ESB自己需要使用的属性信息。
	/// ESB的路由服务将按照消息头中包含的这些属性进行路由判断。
	/// </summary>
	public class MessageHeader : IMessageHeader
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MessageHeader"/> class.
		/// </summary>
		/// <param name="msg">The MSG.</param>
		public MessageHeader(IServiceMessage msg)
		{
			_core = msg;

		}

    
    		IServiceMessage _core;


		/// <summary>Gets the specified name.</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		T Get<T>([CallerMemberName] string name = null)
		{
			object output = null;
			_core.Properties.TryGetValue(name, out output);
			return (T)(output ?? default(T));

		}
		/// <summary>Sets the specified value.</summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <param name="name">The name.</param>
		void Set<T>(T value, [CallerMemberName] string name = null)
		{
			_core.Properties[name] = value;
		}


		/// <summary>
		/// 消息类型
		/// </summary>
		public string MessageType
		{
			get { return Get<string>(); }
			set { Set(value); }
		}
		/// <summary>
		/// 消息ID
		/// </summary>
		public string MessageID
		{
			get { return Get<string>(); }
			set { Set(value); }
		}

	

	}

    	/// <summary>
	/// 
	/// </summary>
    public interface IMessageHeader
    {
    		/// <summary>
		/// 消息类型
		/// </summary>
	    string MessageType
		{
			get;set;
		}
		/// <summary>
		/// 消息ID
		/// </summary>
	    string MessageID
		{
			get;set;
		}

    }
}
