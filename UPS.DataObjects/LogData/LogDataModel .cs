using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.LogData
{

    public class LogInformation
    {
        public string LogRequest { get; set; }
        public string LogResponse { get; set; }
        public string LogException { get; set; }
    }

    public class LogDataModel
    {

        public string userID { get; set; }

        public DateTime dateTime { get; set; }

        public APITypes apiTypes { get; set; }
        public string apiType { get; set; }

        public LogInformation LogInformation { get; set; }
    }

    public enum APITypes
    {
        SFExpress = 1,
        QuincusAPI_Token = 2,
        QuincusAPI_Batch = 3,
        QuincusAPI_Translation = 4,
        AddressBookSetup = 5,
        EFCoreContext = 6,
        ExcelUpload=7,
        Others = 8,
        SF_Payload = 9
    }
}