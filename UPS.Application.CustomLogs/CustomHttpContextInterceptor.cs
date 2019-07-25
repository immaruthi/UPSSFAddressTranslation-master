using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.Application.CustomLogs
{
    public static class CustomHttpContextInterceptor
    {
        private static IHttpContextAccessor iHttpContextAccessor;

        public static Microsoft.AspNetCore.Http.HttpContext Current => iHttpContextAccessor.HttpContext;

        internal static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            iHttpContextAccessor = httpContextAccessor;
        }
    }
}
