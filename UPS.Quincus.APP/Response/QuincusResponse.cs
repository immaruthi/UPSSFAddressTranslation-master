﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.Quincus.APP.Response
{
    public class QuincusResponse
    {
        public List<QuincusReponseData> QuincusReponseDataList { get; set; }
        public QuincusReponseData QuincusReponseData { get; set; }

        public bool ResponseStatus { get; set; }

        public Exception Exception { get; set; }
    }
}
