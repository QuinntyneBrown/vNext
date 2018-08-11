using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Linq;
using vNext.Core.Interfaces;

namespace vNext.API.Features
{
    public class BaseController
    {
        protected readonly IMediator _mediator;
        private readonly string _domain;
        private readonly IDateTime _dateTime;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        private ConcurrentDictionary<string, object> _dictionary = new ConcurrentDictionary<string, object>();

        public BaseController(
            IDateTime dateTime,
            string domain, 
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator) {
            _dateTime = dateTime;
            _domain = domain;
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }
        public ActionResult<TResponse> Submit<TResponse>(int id, IRequest<TResponse> request)
        {
            try
            {
                var @object = _dictionary.GetOrAdd($"{_domain}{id}", new object());

                lock (@object)
                    return _mediator.Send(request).GetAwaiter().GetResult();
                
            }catch(Exception e)
            {
                return new BadRequestObjectResult(e);
            }
        }
    }
}
