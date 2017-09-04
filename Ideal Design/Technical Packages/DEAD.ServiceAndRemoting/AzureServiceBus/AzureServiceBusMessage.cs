using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DEAD.ServiceAndRemoting.Messaging;
using System.Dynamic;
using System.Reflection;
using System.Linq.Expressions;
using DEAD.DependencyInjection;
using DEAD.Logging;
using System.Collections.Concurrent;

namespace DEAD.ServiceAndRemoting.AzureServiceBus
{
    public class AzureServiceBusMessage : IServiceMessage, IDisposable, IIoCContexted
    {


        Microsoft.ServiceBus.Messaging.BrokeredMessage _core;

        protected ExpandoObject _PropertyBags = new ExpandoObject();

        public MessageHeader CommonHeaders { get; private set; }

        internal Microsoft.ServiceBus.Messaging.BrokeredMessage Core
        {
            get { return _core; }
            set
            {
                _core = value;
                this.Properties.Clear();
                this.FillPropertiesFromBrokeredMessageToServiceMessage();
            }
        }



        public AzureServiceBusMessage()
        {
            _core = new BrokeredMessage();
            //this.FillPropertiesFromBrokeredMessageToServiceMessage();
            _Body = new Lazy<object>(
               () =>
                   GetBodyValue(GetBodyType())
            );
            CommonHeaders = new MessageHeader(this);
        }



        public IDictionary<string, object> Properties
        {
            get { return _PropertyBags; }
        }

        object GetBodyValue(Type type)
        {
            if (type == null)
            {
                return null;
            }
            return GetBody(type);
        }
        public AzureServiceBusMessage(object body)
        {
            if (body == null)
            {
                _core = new BrokeredMessage();
            }

            else
            {
                _core = new BrokeredMessage(body);
                HeadersBag.MessageType = body.GetType().GetTypeAsmNameWithNoVersionAndSign();
            }

            this.FillPropertiesFromBrokeredMessageToServiceMessage();

            _Body = new Lazy<object>(
                    () =>
                        GetBodyValue(GetBodyType())
            );
        }

        public AzureServiceBusMessage(BrokeredMessage msg)
        {
            _core = msg ?? new BrokeredMessage();

            this.FillPropertiesFromBrokeredMessageToServiceMessage();

            _Body = new Lazy<object>(
                    () =>
                        GetBodyValue(GetBodyType())
            );
        }//{

        //}



        public void Abandon()
        {
            _core.Abandon();
        }

        public Task AbandonAsync()
        {
            return _core.AbandonAsync();
        }

        public void Complete()
        {
            if (!_IsCompleted)
            {

                try
                {
                    _core.Complete();
                    _IsCompleted = true;
                }
                catch (Exception completeEx)
                {
                    this.ResolveLogger().Critical?.Log("",string.Format("Complete faile,{0}", completeEx));


                }


            }

        }

        public async Task CompleteAsync()
        {
            try
            {


                if (!_IsCompleted)
                {
                    await _core.CompleteAsync();
                    _IsCompleted = true;
                }

            }
            catch (Exception completeEx)
            {

                this.ResolveLogger().Critical?.Log("",string.Format("Complete faile,{0}", completeEx));

            }
        }



        internal bool _IsCompleted;
        public bool IsCompleted
        {
            get { return _IsCompleted; }

        }

        //public override object Body
        //{
        //	get { return _core.GetBody<object>(); }
        //}

        public T GetBody<T>()
        {
            return _core.GetBody<T>();
        }


        static MethodInfo mi = typeof(AzureServiceBusMessage).GetMethods()
            .Where(x => x.Name == "GetBody")
            .Where(x => x.IsGenericMethod)
            .First();


        public object GetBody(Type type)
        {


            Func<AzureServiceBusMessage, object> method = getters.GetOrAdd(type,
                new Func<Type, Func<AzureServiceBusMessage, object>>(k =>
                {
                    var mai = mi.MakeGenericMethod(type);

                    var para = Expression.Parameter(this.GetType(), "inp");
                    var access = Expression.Call(para, mai);
                    var conv = Expression.Convert(access, typeof(object));
                    var findelegate = Expression.Lambda<Func<AzureServiceBusMessage, object>>(conv, para).Compile();

                    return findelegate;
                    //var dg=mai.CreateDelegate(dgtg, this);
                    //dg.DynamicInvoke 

                })
            );

            return method(this);
        }

        static ConcurrentDictionary<Type, Func<AzureServiceBusMessage, object>> getters = new ConcurrentDictionary<Type, Func<AzureServiceBusMessage, object>>();



        Lazy<object> _Body;
        public object Body
        {
            get
            {
                try
                {

                    return _Body.Value;

                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public IDictionary<string, object> Headers
        {
            get
            {
                return _PropertyBags;
            }
        }

        public dynamic HeadersBag
        {
            get
            {
                return _PropertyBags;
            }
        }

        public Type BodyType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public dynamic PropertyBags
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IIoCContext IoCContext
        {
            get; set;
        }

        public Type GetBodyType()
        {
            try
            {
                var msgtp = CommonHeaders.MessageType ?? "";
                var fltred = msgtp.Split(',')
                    .Where(x =>
                    {
                        var item = x.Trim();
                        return !(item.StartsWith("PublicKeyToken") || item.StartsWith("Version"));
                    });
                msgtp = string.Join(",", fltred);
                return Type.GetType(CommonHeaders.MessageType ?? "", false);
            }
            catch (Exception ex)
            {
                this.ResolveLogger().Critical?.Log("",string.Format("Error in  GetBodyType: {0}", ex));

            }

            return null;
        }

        public void Dispose()
        {
            _core.Dispose();
        }
    }
}
