using System;
using System.Net;

namespace UPS.ApplicationProxy
{

    public class WebCredentials : ICredentials
    {
        //public NetworkCredential GetCredential(Uri uri, string authType)
        //{
        //    NetworkCredential networkCredential = new NetworkCredential();
        //    net
        //}
    }

    public class CustomHandler : IWebProxy
    {

        public Uri ProxyUri { get; set; }

        public CustomHandler(string proxyURI)
        {
            this.ProxyUri = new Uri(proxyURI);
        }

        public ICredentials GetCredentials()
        {
            ICredentials credentials = new NetworkCredential("", "");

            return credentials;
        }

        public ICredentials Credentials {
            get;
            set;
        }

        public Uri GetProxy(Uri destination)
        {
            return this.ProxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }


    }
}
