using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEAD.ServiceAndRemoting.AzureServiceBus
{
    public static class TypeNameStringHelper
    {

        public static string GetTypeAsmNameWithNoVersionAndSign(this Type type)
        {
            var name = type.AssemblyQualifiedName;

            var rval = GetTypeAsmNameWithNoVersionAndSign(name);
            return rval;
        }

        public static string GetTypeAsmNameWithNoVersionAndSign(string name)
        {

            var matchCount = 0;
            var chars =
            name.TakeWhile(
                c =>
                {
                    if (c == ',')
                    {
                        matchCount++;
                    }
                    return matchCount < 2;
                }
            ).ToArray();
            var rval = new string(chars);
            return rval;
        }
    }
}
