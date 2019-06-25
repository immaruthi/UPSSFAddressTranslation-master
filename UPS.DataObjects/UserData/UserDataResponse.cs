﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.UserData
{
    public class UserDataResponse
    {
        public IEnumerable<USR> UserData { get; set; }
        public bool Success { get; set; }
        public Exception OperationException { get; set; }
    }
}
