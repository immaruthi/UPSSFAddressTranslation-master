﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.Quincus.APP.Response
{
    public class QuincusTranslatedAddressResponse
    {
        public GetBatchResponseForAddressTranslation ResponseData { get; set; }

        public bool Response { get; set; }

        public Exception exception { get; set; }
    }
}
