using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace DEAD.Logging.Test
{
    [TestClass]
    public class Log4netTest
    {
        [TestMethod]
        public void Log4netChannelTest()
        {
            log4net.Config.XmlConfigurator.Configure();
            var sourceName = Guid.NewGuid().ToString();
            var ll = new DEAD.Logging.Log4net.Log4netStandardChannel(sourceName, StandardLevels.Debug, log4net.LogManager.GetLogger("testApp.Logging"));
            ll?.Log("hahaha");
           
        }
    }
}
