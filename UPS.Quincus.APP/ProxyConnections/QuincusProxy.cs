namespace UPS.Quincus.APP.ProxyConnections
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Cache;
    using Newtonsoft.Json;
    using UPS.Quincus.APP.Common;
    using UPS.Quincus.APP.Configuration;
    using UPS.Quincus.APP.Request;
    using UPS.Quincus.APP.Response;
    using UPS.Quincus.APP.Utilities;

    public class QuincusProxy
    {
        public static QuincusTokenDataResponse GetToken(QuincusParams quincusParams)
        {
            QuincusTokenDataResponse quincusTokenDataResponse = new QuincusTokenDataResponse();

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(quincusParams.endpoint);
                if (string.Equals(MapProxy.WebProxyEnable, true.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    WebProxy myProxy = new WebProxy(MapProxy.webProxyURI, false, null, new NetworkCredential(MapProxy.webProxyUsername, MapProxy.webProxyPassword));
                    httpWebRequest.Proxy = myProxy;
                }
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var input = "{\"username\":\"" + quincusParams.username + "\"," +
                                "\"password\":\"" + quincusParams.password + "\"}";

                    streamWriter.Write(input);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                httpWebRequest.KeepAlive = false;
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                string response;

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                    streamReader.Close();
                }

                if (!string.IsNullOrWhiteSpace(response))
                {
                    quincusTokenDataResponse.quincusTokenData = JsonConvert.DeserializeObject<QuincusTokenData>(response);
                    quincusTokenDataResponse.ResponseStatus = true;
                }

                httpResponse.Close();

            }
            catch(Exception exception)
            {
                quincusTokenDataResponse.exception = exception;
            }

            return quincusTokenDataResponse;
        }


        public static QuincusTranslatedAddressResponse GetTranslatedAddressResponse(IQuincusAddressTranslationRequest quincusAddressTranslationRequest)
        {
            string response = string.Empty;
            QuincusTranslatedAddressResponse quincusTranslatedAddressResponse = new QuincusTranslatedAddressResponse();

            try
            {
                string content = GetRequestContextForAddress.GetAddressStringFromRequest(quincusAddressTranslationRequest.shipmentWorkFlowRequests);

                if (!string.IsNullOrWhiteSpace(content))
                {

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(
                        quincusAddressTranslationRequest.endpoint);
                    if (string.Equals(MapProxy.WebProxyEnable,true.ToString(),StringComparison.OrdinalIgnoreCase))
                    {
                        WebProxy myProxy = new WebProxy(MapProxy.webProxyURI, false, null, new NetworkCredential(MapProxy.webProxyUsername, MapProxy.webProxyPassword));

                        httpWebRequest.Proxy = myProxy;
                    }

                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Headers.Add("AUTHORIZATION", "JWT " + quincusAddressTranslationRequest.token);
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        var input = content;

                        streamWriter.Write(input);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        response = streamReader.ReadToEnd();

                        streamReader.Close();
                    }

                    httpResponse.Close();

                    quincusTranslatedAddressResponse.ResponseData = JsonConvert.DeserializeObject<GetBatchResponseForAddressTranslation>(response);
                    quincusTranslatedAddressResponse.Response = true;
                }
                else
                {

                }
            }
            catch(Exception exception)
            {
                quincusTranslatedAddressResponse.exception = exception;
            }

            return quincusTranslatedAddressResponse;            
        }

        public static QuincusResponse GetQuincusResponse(QuincusGeoCodeDataRequest quincusGeoCodeDataRequest)
        {
            bool retryflag = true;
            int retryCount = 0;
            QuincusResponse quincusResponse = new QuincusResponse();
            HttpWebResponse httpResponse = null;

            try
            {
                while (retryflag && retryCount<=3)
                {

                    HttpRequestCachePolicy requestCachePolicy =
                            new HttpRequestCachePolicy(HttpRequestCacheLevel.Default);

                    HttpWebRequest.DefaultCachePolicy = requestCachePolicy;

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(
                        quincusGeoCodeDataRequest.endpoint + quincusGeoCodeDataRequest.id + "/");

                    HttpRequestCachePolicy noCachePolicy =
                        new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);

                    httpWebRequest.CachePolicy = noCachePolicy;

                    if (string.Equals(MapProxy.WebProxyEnable, true.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        WebProxy myProxy = new WebProxy(MapProxy.webProxyURI, false, null, new NetworkCredential(MapProxy.webProxyUsername, MapProxy.webProxyPassword));
                        httpWebRequest.Proxy = myProxy;
                    }

                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Headers.Add("AUTHORIZATION", "JWT " + quincusGeoCodeDataRequest.quincusTokenData.token);
                    httpWebRequest.Method = "GET";
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    if (string.Equals(httpResponse.StatusDescription, "No Content", StringComparison.OrdinalIgnoreCase))
                    {
                        System.Threading.Thread.Sleep(5000);
                        retryCount++;
                    }
                    else
                    {
                        retryflag = false;
                    }
                }

                string response;

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    response = streamReader.ReadToEnd();
                }

                if (!string.IsNullOrEmpty(response))
                {
                    quincusResponse.QuincusReponseData = Newtonsoft.Json.JsonConvert.DeserializeObject<QuincusReponseData>(response);
                    quincusResponse.ResponseStatus = true;
                }
            }
            catch(Exception exception)
            {
                quincusResponse.Exception = exception;
            }

            return quincusResponse;
        }
    }
}
