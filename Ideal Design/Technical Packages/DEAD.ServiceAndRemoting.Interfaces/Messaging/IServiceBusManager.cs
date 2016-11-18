
using DEAD.ServiceAndRemoting.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.Messaging
{
	/// <summary>
	/// 
	/// </summary>
	public interface IServiceBusManager
	{
		/// <summary>Gets the entry asynchronous.</summary>
		/// <typeparam name="TServiceBusEntry">The type of the service bus entry.</typeparam>
		/// <param name="path">The path.</param>
		/// <param name="IsPeekAndNeedComplete">if set to <c>true</c> [is peek and need complete].</param>
		/// <returns></returns>
		//Task<TServiceBusEntry> GetEntryAsync<TServiceBusEntry>(string path = null, bool IsPeekAndNeedComplete = false) where TServiceBusEntry : class , IServiceBusEntry;

		/// <summary>Gets the entry.</summary>
		/// <typeparam name="TServiceBusEntry">The type of the service bus entry.</typeparam>
		/// <param name="path">The path.</param>
		/// <param name="IsPeekAndNeedComplete">if set to <c>true</c> [is peek and need complete].</param>
		/// <returns></returns>
		//TServiceBusEntry GetEntry<TServiceBusEntry>(string path = null, bool IsPeekAndNeedComplete = false) where TServiceBusEntry : class , IServiceBusEntry;

		/// <summary>Gets or sets the current authentication token.</summary>
		/// <value>The current authentication token.</value>
		//IAuthToken CurrentAuthToken { get; set; }
		/// <summary>Gets or sets the current authentication checker.</summary>
		/// <value>The current authentication checker.</value>
		//IAuthChecker CurrentAuthChecker { get; set; }

		/// <summary>Gets or sets the name of the service bus setting.</summary>
		/// <value>The name of the service bus setting.</value>
		string ServiceBusSettingName { get; set; }


		/// <summary>Creates the message.</summary>
		/// <param name="body">The body.</param>
		/// <returns></returns>
		IServiceMessage CreateMessage(object body);



		/// <summary>Gets the address.</summary>
		/// <value>The address.</value>
		Uri Address { get; }
	}


}
