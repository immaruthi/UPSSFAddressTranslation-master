using System;
using System.Collections.Generic;
using UPS.DataObjects.Shipment;
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

        public static QuincusTranslatedAddressResponse GetTranslationAddress(IQuincusAddressTranslationRequest quincusAddressTranslationRequest)
        {

            QuincusTranslatedAddressResponse quincusTranslatedAddressResponse = QuincusProxy.GetTranslatedAddressResponse(quincusAddressTranslationRequest);

            return quincusTranslatedAddressResponse;
        }

        public static QuincusResponse GetGeoCodeReponseFromQuincus(QuincusGeoCodeDataRequest quincusGeoCodeDataRequest, decimal shipmentsCount = 10)
        {
            QuincusResponse quincusReponse = QuincusProxy.GetQuincusResponse(quincusGeoCodeDataRequest,shipmentsCount);
            return quincusReponse;
        }

        public static GetSFCreateOrderServiceResponse SFExpressCreateOrder(SFCreateOrderServiceRequest sFCreateOrderServiceRequest)
        {
            GetSFCreateOrderServiceResponse getSFCreateOrderServiceResponse =  new SFExpressProxy().getSFCreateOrderServiceResponse(sFCreateOrderServiceRequest).Result;

            return getSFCreateOrderServiceResponse;
        }

        public static GetSFCancelOrderServiceResponse SFExpressCancelOrder(SFCancelOrderServiceRequest sFCancelOrderServiceRequest)
        {
            GetSFCancelOrderServiceResponse getSFCancelOrderServiceResponse = new SFExpressProxy().getSFCancelOrderServiceResponse(sFCancelOrderServiceRequest).Result;

            return getSFCancelOrderServiceResponse;
        }

    }
}
