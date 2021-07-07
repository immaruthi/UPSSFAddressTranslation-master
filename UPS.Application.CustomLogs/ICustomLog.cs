using Quincus.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UPS.DataObjects.LogData;

namespace UPS.Application.CustomLogs
{
    public interface ICustomLog:IAction
    {
        Task AddLogEntry(LogDataModel logDataModel);

        string[] GetLogFileList();

        LogDataModel[] ReadLogFileData(string filePath);
    }
}
