using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.Interfaces
{
	public interface IMessageReceiver
	{
		Task<IMessage> ReceiveMessageAsync(Int32 waitMS =3000);
		Task<IMessage[]> ReceiveMessagesAsync(Int32 waitMS = 3000);	   

	}
}
