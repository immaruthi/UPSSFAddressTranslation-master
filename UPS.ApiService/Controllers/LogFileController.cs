using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtService.HeadController;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UPS.Application.CustomLogs;
using UPS.DataObjects.LogData;
using UPS.ServicesDataRepository;

namespace AtService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogFileController : UPSController
    {

        public ICustomLog iCustomLog { get; set; }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetLogFiles()
        {

            return Ok(iCustomLog.GetLogFileList());

            //LogFileService logFileService = new LogFileService();
            //List<string> LogFiles = new List<string>();
            //LogFiles = logFileService.GetLogFileList();
            //return LogFiles;
        }

        [Route("ReadFileData")]
        [HttpGet("[action]")]
        public async Task<ActionResult> ReadLogFileData(string filePath)
        {
            return Ok(iCustomLog.ReadLogFileData(filePath));
        }
    }
}