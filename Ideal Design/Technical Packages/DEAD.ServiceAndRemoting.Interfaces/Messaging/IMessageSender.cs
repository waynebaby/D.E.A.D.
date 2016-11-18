using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.Messaging
{
	public interface IMessageSender
	{						   
		Task SendMessageAsync(IServiceMessage message);
		Task SendMessagesAsync(params IServiceMessage[] messages);


        void SendMessage(IServiceMessage message);
        void SendMessages(params IServiceMessage[] messages);
    }
}
