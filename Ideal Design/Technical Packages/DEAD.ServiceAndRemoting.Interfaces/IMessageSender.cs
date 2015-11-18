using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.Interfaces
{
	public interface IMessageSender
	{						   
		Task SendMessageAsync(IMessage message);
		Task SendMessagesAsync(params IMessage[] messages);	  
	}
}
