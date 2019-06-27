using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace UPS.Application.CustomLogs
{
    public class AuditEventEntry
    {
       
        public static void WriteEntry(Exception exception)
        {
            using (var myLog = new EventLog())
            {
                myLog.Source = "Application";   
                myLog.WriteEntry(exception.Message, EventLogEntryType.Error);
            }
        }
    }
}
