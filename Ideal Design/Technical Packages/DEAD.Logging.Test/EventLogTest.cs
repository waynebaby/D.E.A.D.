using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace DEAD.Logging.Test
{
    [TestClass]
    public class EventLogTest
    {
        [TestMethod]
        public void EventLogChannelTest()
        {
            var sourceName = Guid.NewGuid().ToString();
            if (!System.Diagnostics.EventLog.SourceExists(sourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(sourceName, "Application");

            }

            var el = new DEAD.Logging.EventLog.EventLogStandardChannel(sourceName, StandardLevels.Debug);
            
            el?.Log("hahaha");
            Thread.Sleep(100);
            var elcore = new System.Diagnostics.EventLog("Application",".");
            
            var e = elcore.Entries[elcore.Entries.Count-1];
            Assert.AreEqual(sourceName, e.Source);
            if (System.Diagnostics.EventLog.SourceExists(sourceName))
            {


                System.Diagnostics.EventLog.DeleteEventSource(sourceName);
            }
        }
    }
}
