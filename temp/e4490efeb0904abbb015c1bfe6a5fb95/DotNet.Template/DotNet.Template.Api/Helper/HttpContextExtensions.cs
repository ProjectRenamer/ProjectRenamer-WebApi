using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DotNet.Template.Api.Helper
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            return httpContext?.User?.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public static IEnumerable<string> GetRoles(this HttpContext httpContext)
        {
            return httpContext?.User?.Claims?.Where(claim => claim.Type == ClaimTypes.Role)?.Select(claim => claim.Value) ?? new string[] { };
        }
    }
}
