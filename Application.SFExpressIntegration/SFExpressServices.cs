using Application.SFExpressIntegration.ProxyActions;
using Application.SFExpressIntegration.SFExpressServiceData;
using System;

namespace Application.SFExpressIntegration
{
    public class SFExpressServices
    {
        public static SFExpressServiceResponseData CreateShipment(SFExpressServiceRequestData sFExpressServiceRequestData)
        {
            SFExpressServiceResponseData sFExpressServiceResponseData = 
                SFServerProxy.GetsFExpressServiceResponseData(sFExpressServiceRequestData);

            return sFExpressServiceResponseData;
        }
    }
}
