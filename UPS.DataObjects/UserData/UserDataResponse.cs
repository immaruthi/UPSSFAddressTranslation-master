using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.DataObjects.UserData
{
    public class UserDataResponse
    {
        public User User { get; set; }
        public IEnumerable<User> UserData { get; set; }
        public bool Success { get; set; }
        public Exception OperationException { get; set; }
        public string Message { get; set; }
    }
}
