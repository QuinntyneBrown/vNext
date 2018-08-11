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
            var context = _httpContextAccessor.HttpContext;

            switch(request)
            {
                case IAuthenticatedRequest authenticatedRequest:
                    authenticatedRequest.UserId = Convert.ToInt32(context.User.Claims.Single(x => x.Type == "UserId").Value);
                    authenticatedRequest.CurrentDateTime = _dateTime.UtcNow;
                    authenticatedRequest.CustomerKey = $"{context.User.Claims.Single(x => x.Type == "CustomerKey").Value}";
                    break;
            }

            return next();
        }
    }
}
