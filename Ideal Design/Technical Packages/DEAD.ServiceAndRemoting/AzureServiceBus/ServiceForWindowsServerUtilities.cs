using DEAD.ServiceAndRemoting.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.AzureServiceBus
{
    internal static class ServiceForWindowsServerUtilities
    {
        public static void FillPropertiesFromServiceMessageToBrokeredMessage(this IServiceMessage msg)
        {
            var m = (AzureServiceBusMessage)msg;

            var t = m.Core;

            var dicF = m.Properties;
            var dicT = t.Properties;

            CopyObjectsAmongDics(dicF, dicT);
        }

        private static void CopyObjectsAmongDics(IDictionary<string, object> dicF, IDictionary<string, object> dicT)
        {
            foreach (var kvp in dicF)
            {
                dicT[kvp.Key] = kvp.Value;

            }
        }
        public static void FillPropertiesFromBrokeredMessageToServiceMessage(this IServiceMessage msg)
        {
            var m = (AzureServiceBusMessage)msg;

            var t = m.Core;

            var dicT = m.Properties;
            var dicF = t.Properties;

            CopyObjectsAmongDics(dicF, dicT);
        }

    }
}
