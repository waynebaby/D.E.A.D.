using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.Messaging
{
	/// <summary>
	/// 偷看锁支持
	/// </summary>
	public interface IPeekLockToken : IDisposable
	{
		/// <summary>
		/// 取出信息且完成偷看	 
		/// </summary>
		void Complete();

		/// <summary>
		/// 取出信息且完成偷看  
		/// </summary>
		Task CompleteAsync();

		/// <summary>
		/// 放回信息取消偷看
		/// </summary>
		void Abandon();

		/// <summary>
		/// 放回信息取消偷看
		/// </summary>
		Task AbandonAsync();

		/// <summary>
		/// 是否已经完全取出
		/// </summary>
		bool IsCompleted { get; }

	}
}
