using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AtService.ProxyChannel
{

    

    public class UPSCustomProxy : IWebProxy
    {
        private readonly IConfiguration Configuration;

        public UPSCustomProxy(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public ICredentials Credentials
        {
            get { return new NetworkCredential(Configuration["webProxy:Username"], Configuration["webProxy:Password"]); }
            set { }
        }

        public Uri GetProxy(Uri destination)
        {
            return new Uri(Configuration["webProxy:URL"]);
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}
