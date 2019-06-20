using System;
using UPS.Quincus.APP.Configuration;
using UPS.Quincus.APP.ProxyConnections;
using UPS.Quincus.APP.Request;
using UPS.Quincus.APP.Response;

namespace UPS.Quincus.APP
{
    public class QuincusService
    {

        public static QuincusTokenDataResponse GetToken(QuincusParams quincusParams)
        {
            QuincusTokenDataResponse quincusTokenDataResponse = QuincusProxy.GetToken(quincusParams);
            return quincusTokenDataResponse;

        }

        public static QuincusResponse GetGeoCodeReponseFromQuincus(QuincusGeoCodeDataRequest quincusGeoCodeDataRequest)
        {
            QuincusResponse quincusReponse = QuincusProxy.GetQuincusResponse(quincusGeoCodeDataRequest);
            return quincusReponse;
        }

    }
}
