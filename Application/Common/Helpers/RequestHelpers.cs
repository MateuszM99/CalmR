using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Application.Common.Helpers
{
    public static class RequestHelpers
    {
        public static string GetIpAddress(HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        public static string GetOrigin(HttpContext httpContext)
        {
            return httpContext.Request.Headers["Origin"];
        }
    }
}