using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using vNext.Core.Identity;

namespace vNext.Core.Middleware
{
    public class AutoAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISecurityTokenFactory _securityTokenFactory;

        public AutoAuthenticationMiddleware(ISecurityTokenFactory securityTokenFactory, RequestDelegate next)
        {
            _next = next;
            _securityTokenFactory = securityTokenFactory;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var token = _securityTokenFactory.Create("Comsense1",2);
            httpContext.Request.Headers.Add("Authorization", $"Bearer {token}");
            await _next.Invoke(httpContext);
        }
    }
}
