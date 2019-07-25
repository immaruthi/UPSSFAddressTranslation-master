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
using UPS.ServicesDataRepository.Common;
using System.Threading.Tasks;

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
                try
                {
                    logDataModel.userID = CustomHttpContextInterceptor.Current.User.Claims.FirstOrDefault(x => x.Type == JwtConstant.UserIdText).Value;
                }
                catch(Exception ex)
                {
                    logDataModel.userID = "Not Found";
                }

                sw.WriteLine(JsonConvert.SerializeObject(logDataModel) + ",");
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task AddLogEntry(LogDataModel logDataModel)
        {
            return Task.Run(() => LogEntry(logDataModel));
        }

        public static string[] GetLogFiles()
        {

            List<string> fileNames = new List<string>();

            Directory.GetFiles(HostingEnvironment.WebRootPath, "*.txt",
                                         SearchOption.TopDirectoryOnly).ToList().ForEach(filename =>
                                         {
                                             fileNames.Add(Path.GetFileName(filename));
                                         });

            fileNames.Reverse();

            return fileNames.ToArray();
        }

        public static LogDataModel[] GetLogFileDataFromFileName(string filePath)
        {
            try
            {


                string jsonText = File.ReadAllText(filePath.ToString());

                jsonText = "[" + jsonText + "]";


                var logDataModels = JsonConvert.DeserializeObject<LogDataModel[]>(jsonText).OrderByDescending(date => date.dateTime);


                return logDataModels.ToArray();
            } 
            catch(Exception ex)
            {
                LogDataModel logDataModel = new LogDataModel();
                logDataModel.LogInformation = new LogInformation()
                {
                    LogException = ex.Message.ToString(),
                    LogRequest = "Request File Found " + filePath,
                    LogResponse = "Please contact support"
                };

                LogDataModel[] logDataModels = new LogDataModel[1] { logDataModel };

                return logDataModels;
            }
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
