using DEAD.ServiceAndRemoting.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.Messaging
{
    public interface IMessageReceiver
    {

        Task<IServiceMessage> ReceiveMessageAsync(Int32 waitMS = Consts.DefaultWaitMs);
        Task<IList<IServiceMessage>> ReceiveMessagesAsync(Int32 waitMS = Consts.DefaultWaitMs);

        IServiceMessage ReceiveMessage(Int32 waitMS = Consts.DefaultWaitMs);
        IList<IServiceMessage> ReceiveMessages(Int32 waitMS = Consts.DefaultWaitMs);

        IList<IServiceMessage> ReceiveMessages(int fetchAmount, int millisecondsBuffer);

        Task<IList<IServiceMessage>> ReceiveMessagesAsync(int fetchAmount, int millisecondsBuffer);

    }

    public static class Consts
    {
        public const int DefaultWaitMs = 3000;
        public const int DefaultMaxMessagesCountPerFetch = 100;
    }
}
