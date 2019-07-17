using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using UPS.DataObjects.LogData;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace UPS.Application.CustomLogs
{
    public class AuditEventEntry: ICustomLog, IDisposable
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
            try
            {
                filePath = Path.Combine(HostingEnvironment.WebRootPath, string.Format(Configuration["APILogger:logFileName"], System.DateTime.Now.ToString("yyyyMMdd")));

                if (!File.Exists(filePath))
                {
                    using (FileStream fs = File.Create(filePath))
                    {

                    }
                }
            }
            catch(Exception ex)
            {
                WriteEntry(ex);
            }
        }

        public static void LogEntry(LogDataModel logDataModel)
        {
            LogInit();
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(JsonConvert.SerializeObject(logDataModel) + ",");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void AddLogEntry(LogDataModel logDataModel)
        {
            LogEntry(logDataModel);
        }

        public static string[] GetLogFiles()
        {

            List<string> fileNames = new List<string>();

            Directory.GetFiles(HostingEnvironment.WebRootPath, "*.txt",
                                         SearchOption.TopDirectoryOnly).ToList().ForEach(filename =>
                                         {
                                             fileNames.Add(Path.GetFileName(filename));
                                         });
            
            return fileNames.ToArray();
        }

        public static LogDataModel[] GetLogFileDataFromFileName(string filePath)
        {

            string jsonText = File.ReadAllText(filePath.ToString());

            jsonText = "[" + jsonText + "]";


            var logDataModels = JsonConvert.DeserializeObject<LogDataModel[]>(jsonText);


            return logDataModels;
        }

        public string[] GetLogFileList()
        {
            return GetLogFiles();
        }

        public LogDataModel[] ReadLogFileData(string filename)
        {
            return GetLogFileDataFromFileName(Path.Combine(HostingEnvironment.WebRootPath, filename));
        }
    }
}
