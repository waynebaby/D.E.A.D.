using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DEAD.Logging.Test
{
    [TestClass]
    public class EventLogTest
    {
        [TestMethod]
        public void EventLoggerTest()
        {
            var sourceName = "aa2a";
            if (!System.Diagnostics.EventLog.SourceExists(sourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(sourceName, "Application");

            }

            var el = new DEAD.Logging.EventLog.EventLogStandardChannel(sourceName, StandardLevels.Debug);
            el?.Log("hahaha");

        }
    }
}
