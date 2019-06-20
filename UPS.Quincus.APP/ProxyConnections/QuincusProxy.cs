using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UPS.Quincus.APP.Configuration;
using UPS.Quincus.APP.Request;
using UPS.Quincus.APP.Response;

namespace UPS.Quincus.APP.ProxyConnections
{
    public class QuincusProxy
    {
        public static QuincusTokenDataResponse GetToken(QuincusParams quincusParams)
        {
            QuincusTokenDataResponse quincusTokenDataResponse = new QuincusTokenDataResponse();

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(quincusParams.endpoint);
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
                }

                if (!string.IsNullOrWhiteSpace(response))
                {
                    quincusTokenDataResponse.quincusTokenData = JsonConvert.DeserializeObject<QuincusTokenData>(response);
                    quincusTokenDataResponse.ResponseStatus = true;
                }

            }
            catch(Exception exception)
            {
                quincusTokenDataResponse.exception = exception;
            }

            return quincusTokenDataResponse;
        }


        public static QuincusResponse GetQuincusResponse(QuincusGeoCodeDataRequest quincusGeoCodeDataRequest)
        {
            QuincusResponse quincusResponse = new QuincusResponse();

            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(
                    quincusGeoCodeDataRequest.endpoint + quincusGeoCodeDataRequest.id + "/");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("AUTHORIZATION", "JWT " + quincusGeoCodeDataRequest.quincusTokenData.token);
                httpWebRequest.Method = "GET";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
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
