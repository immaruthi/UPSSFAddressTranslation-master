using Application.SFExpressIntegration.SFExpressServiceData;
using SFServicesChannel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.SFExpressIntegration.ProxyActions
{
    public class SFServerProxy
    {
        //ExpressServiceClient

        public static SFExpressServiceResponseData GetsFExpressServiceResponseData(SFExpressServiceRequestData sFExpressServiceRequestData)
        {
            SFExpressServiceResponseData sFExpressServiceResponseData = new SFExpressServiceResponseData();

            ExpressServiceClient expressServiceClient = new ExpressServiceClient();

            try
            {
                string combineXMLRequest_CheckWord = sFExpressServiceRequestData.OrderRequest + sFExpressServiceRequestData.Checkword;

                System.Security.Cryptography.MD5 hs = System.Security.Cryptography.MD5.Create();

                byte[] base64Encryption = hs.ComputeHash(System.Text.Encoding.UTF8.GetBytes(combineXMLRequest_CheckWord));

                string base64VeirificatioCode = Convert.ToBase64String(base64Encryption);

                sFExpressServiceResponseData.SFExpressServiceResponse = 
                    Task.Run(
                        () => new ExpressServiceClient()
                        .sfexpressServiceAsync(
                            sFExpressServiceRequestData.OrderRequest, base64VeirificatioCode))
                            .Result.ToString();


                sFExpressServiceResponseData.Response = true;
            }
            catch(Exception exception)
            {
                sFExpressServiceResponseData.exception = exception;
            }

            return sFExpressServiceResponseData;

        }

    }
}
