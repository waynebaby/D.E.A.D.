using DEAD.DependencyInjection;
using DEAD.ServiceAndRemoting.Messaging;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.AzureServiceBus
{

    public class AzureMessageReceiver : IMessageReceiver, IIoCContexted
    {
        int _maxMessagePerFetch;
        MessageReceiver _Client;
        public AzureMessageReceiver(MessageReceiver receiver, int maxMessagePerFetch = Consts.DefaultMaxMessagesCountPerFetch)
        {
            _Client = receiver;
            _maxMessagePerFetch = maxMessagePerFetch;
        }


        public IServiceMessage ReceiveMessage()
        {
            var msg = _Client.Receive();
            AzureServiceBusMessage newMSG = CreateNewServiceMessage(msg);

            return newMSG;
        }

        private AzureServiceBusMessage CreateNewServiceMessage(BrokeredMessage msg)
        {
            var newMSG = this.GetIoCManager().DefualtContainer.Resolve<AzureServiceBusMessage>();
            newMSG.Core = msg;
            newMSG._IsCompleted = _Client.Mode == ReceiveMode.ReceiveAndDelete;
            return newMSG;
        }

        public IServiceMessage ReceiveMessage(int waitMS = Consts.DefaultWaitMs)
        {
            var msg = _Client.Receive(TimeSpan.FromMilliseconds(waitMS));
            return CreateNewServiceMessage(msg);
        }

        public async Task<IServiceMessage> ReceiveMessageAsync()
        {
            return await ReceiveMessageAsync(_maxMessagePerFetch);
        }

        public IList<IServiceMessage> ReceiveMessages(int fetchAmount, int millisecondsBuffer)
        {
            var msgs = _Client.ReceiveBatch(fetchAmount, TimeSpan.FromMilliseconds(millisecondsBuffer));
            return msgs.Select(msg => CreateNewServiceMessage(msg)).ToArray();
        }

        public async Task<IList<IServiceMessage>> ReceiveMessagesAsync(int fetchAmount, int millisecondsBuffer)
        {
            var msgs = await _Client.ReceiveBatchAsync(fetchAmount, TimeSpan.FromMilliseconds(millisecondsBuffer));
            return msgs.Select(msg => CreateNewServiceMessage(msg)).ToArray();
        }

        public async Task<IServiceMessage> ReceiveMessageAsync(int waitMS = 3000)
        {
            BrokeredMessage msg = null;
            while (msg == null)
            {
                msg = await _Client.ReceiveAsync(TimeSpan.FromMilliseconds(waitMS));
            }

            return CreateNewServiceMessage(msg);
        }

        public async Task<IList<IServiceMessage>> ReceiveMessagesAsync(int waitMS = Consts.DefaultWaitMs)
        {
            return await ReceiveMessagesAsync(_maxMessagePerFetch, waitMS);
        }

        public IList<IServiceMessage> ReceiveMessages(int waitMS = Consts.DefaultWaitMs)
        {
            return ReceiveMessages(Consts.DefaultMaxMessagesCountPerFetch, Consts.DefaultWaitMs);
        }

        public string Name
        {
            get;
            internal set;
        }

        public string Path
        {
            get { return _Client.Path; }
        }



        public IServiceBusManager ServiceBusManager
        {
            get;
            set;
        }

        public IIoCContext IoCContext
        {
            get; set;
        }
    }


}
