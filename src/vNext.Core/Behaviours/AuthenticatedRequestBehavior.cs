using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.Interfaces;

namespace vNext.Core.Behaviours
{
    public class AuthenticatedRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDateTime _dateTime;

        public AuthenticatedRequestBehavior(IDateTime dateTime,IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dateTime = dateTime;
        }
        
        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims;

            switch(request)
            {
                case IAuthenticatedRequest authenticatedRequest:
                    authenticatedRequest.CurrentUserId = Convert.ToInt32(claims.Single(x => x.Type == "UserId").Value);
                    authenticatedRequest.CurrentDateTime = _dateTime.UtcNow;
                    authenticatedRequest.CustomerKey = $"{claims.Single(x => x.Type == "CustomerKey").Value}";
                    break;
            }

            return next();
        }
    }
}
