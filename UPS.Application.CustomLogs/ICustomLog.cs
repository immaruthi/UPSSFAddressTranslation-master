using Quincus.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using UPS.DataObjects.LogData;

namespace UPS.Application.CustomLogs
{
    public interface ICustomLog:IAction
    {
        void AddLogEntry(LogDataModel logDataModel);
    }
}
