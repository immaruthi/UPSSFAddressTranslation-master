using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using UPS.DataObjects.LogData;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace UPS.Application.CustomLogs
{
    public class AuditEventEntry:IDisposable
    { 

        public static IConfiguration Configuration { get; set; }
        public static IHostingEnvironment HostingEnvironment { get; set; }
        private static string filePath { get; set; }

        public static void WriteEntry(Exception exception)
        {
            using (var myLog = new EventLog())
            {
                myLog.Source = "Application";   
                myLog.WriteEntry(exception.Message, EventLogEntryType.Error);
            }
        }

        private static void LogInit()
        {
            filePath = Path.Combine(HostingEnvironment.WebRootPath, string.Format(Configuration["APILogger:logFileName"], System.DateTime.Now.ToString("yyyyMMdd")));

            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {

                }
            }
        }

        public static void LogEntry(LogDataModel logDataModel)
        {
            LogInit();
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(JsonConvert.SerializeObject(logDataModel) + "," + System.Environment.NewLine);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
