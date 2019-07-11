using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using UPS.DataObjects.LogData;
using Newtonsoft.Json;

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


        public static void LogEntry(LogDataModel logDataModel, string path)
        {

            LogDataModel logDataModelData = new LogDataModel()
            {
                apiTypes = APITypes.QuincusAPI_Translation,
                dateTime = System.DateTime.Now,
                LogInformation = new LogInformation()
                {
                    LogException = new Exception("Test Message"),
                    LogRequest = "Test Log Request",
                    LogResponse = "Test Lod Response"
                }
            };

            String JsonData = string.Empty;

            for (int i = 0; i < 2; i++)
            {

                JsonData += JsonConvert.SerializeObject(logDataModelData) + ",";
            }

            JsonData = JsonData.Remove(JsonData.Length - 1);

            JsonData = "[" + JsonData + "]";



            LogDataModel[] logDataModels = JsonConvert.DeserializeObject<LogDataModel[]>(JsonData);


            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(JsonData + System.Environment.NewLine);
            }
        }
    }
}
