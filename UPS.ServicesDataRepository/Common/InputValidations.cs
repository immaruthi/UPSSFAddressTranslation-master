using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.ServicesDataRepository.Common
{
    public class InputValidations
    {
        public static bool IsDecimalFormat(string input)
        {
            Decimal dummy;
            return Decimal.TryParse(input, out dummy);
        }
    }
}
