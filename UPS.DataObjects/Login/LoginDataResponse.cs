using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.Login
{
    public class LoginDataResponse
    {
        public IEnumerable<LoginData> Catalogs { get; set; }
        public bool Success { get; set; }
        public Exception OperationException { get; set; }

    }
}
