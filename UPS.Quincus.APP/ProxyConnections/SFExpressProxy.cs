namespace UPS.Quincus.APP.ProxyConnections
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using UPS.Quincus.APP.Common;
    using UPS.Quincus.APP.Request;
    using UPS.Quincus.APP.Response;

    public class SFExpressProxy
    {

        private HttpClientHandler GetHttpClientHandler()
        {
            WebProxy myProxy = new WebProxy(MapProxy.webProxyURI, false, null, new NetworkCredential(MapProxy.webProxyUsername, MapProxy.webProxyPassword));
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.Proxy = myProxy;

            return httpClientHandler;
        }

        public async Task<GetSFCreateOrderServiceResponse> getSFCreateOrderServiceResponse(SFCreateOrderServiceRequest sFCreateOrderServiceRequest)
        {
            GetSFCreateOrderServiceResponse getSFCreateOrderServiceResponse = new GetSFCreateOrderServiceResponse();
            //string verifyText = sFCreateOrderServiceRequest.Checkword;
            try
            {
                string toVerifyText = sFCreateOrderServiceRequest.RequestOrderXMLMessage + sFCreateOrderServiceRequest.Checkword;

                System.Security.Cryptography.MD5 hs = System.Security.Cryptography.MD5.Create();

                byte[] base64Encryption = hs.ComputeHash(System.Text.Encoding.UTF8.GetBytes(toVerifyText));

                string base64VeirificatioCode = Convert.ToBase64String(base64Encryption);

                Dictionary<String, String> map = new Dictionary<string, string>();
                map.Add("xml", sFCreateOrderServiceRequest.RequestOrderXMLMessage);
                map.Add("verifyCode", base64VeirificatioCode);
                map.Add("checkCode", sFCreateOrderServiceRequest.Checkword);

                IEnumerable<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                keyValuePairs = map;

                string resultContent = string.Empty;
                HttpClient httpClient = null;

                if (string.Equals(MapProxy.WebProxyEnable, false.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    httpClient = new HttpClient();
                }
                else
                {
                    httpClient = new HttpClient(GetHttpClientHandler());
                }

                using (var client = httpClient)
                {
                    client.BaseAddress = new Uri(sFCreateOrderServiceRequest.BaseURI);
                    var content = new FormUrlEncodedContent(keyValuePairs);


                    var result = await client.PostAsync(sFCreateOrderServiceRequest.RequestURI, content);

                    resultContent = await result.Content.ReadAsStringAsync();

                    getSFCreateOrderServiceResponse.OrderResponse = resultContent;
                    getSFCreateOrderServiceResponse.Response = true;

                }
            }
            catch(Exception exception)
            {
                getSFCreateOrderServiceResponse.exception = exception;
            }

            return getSFCreateOrderServiceResponse;
        }

        public async Task<GetSFCancelOrderServiceResponse> getSFCancelOrderServiceResponse(SFCancelOrderServiceRequest sFCancelOrderServiceRequest)
        {
            GetSFCancelOrderServiceResponse getSFCancelOrderServiceResponse = new GetSFCancelOrderServiceResponse();

            try
            {
                string toVerifyText = sFCancelOrderServiceRequest.RequestOrderXMLMessage + sFCancelOrderServiceRequest.Checkword;

                System.Security.Cryptography.MD5 hs = System.Security.Cryptography.MD5.Create();

                byte[] base64Encryption = hs.ComputeHash(System.Text.Encoding.UTF8.GetBytes(toVerifyText));

                string base64VeirificatioCode = Convert.ToBase64String(base64Encryption);

                Dictionary<String, String> map = new Dictionary<string, string>();
                map.Add("xml", sFCancelOrderServiceRequest.RequestOrderXMLMessage);
                map.Add("verifyCode", base64VeirificatioCode);
                map.Add("checkCode", sFCancelOrderServiceRequest.Checkword);

                IEnumerable<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                keyValuePairs = map;

                string resultContent = string.Empty;

                HttpClient httpClient = null;

                if (string.Equals(MapProxy.WebProxyEnable, false.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    httpClient = new HttpClient();
                }
                else
                {
                    httpClient = new HttpClient(GetHttpClientHandler());
                }

                using (var client = httpClient) 
                {
                    client.BaseAddress = new Uri(sFCancelOrderServiceRequest.BaseURI);
                    var content = new FormUrlEncodedContent(keyValuePairs);


                    var result = await client.PostAsync(sFCancelOrderServiceRequest.RequestURI, content);

                    resultContent = await result.Content.ReadAsStringAsync();

                    getSFCancelOrderServiceResponse.OrderResponse = resultContent;
                    getSFCancelOrderServiceResponse.Response = true;

                }
            }
            catch (Exception exception)
            {
                getSFCancelOrderServiceResponse.exception = exception;
            }

            return getSFCancelOrderServiceResponse;
        }
    }
}
