using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace vNext.API.Features
{
    public class BaseController
    {
        protected readonly IMediator _mediator;
        private readonly string _domain;

        private ConcurrentDictionary<string, object> _dictionary = new ConcurrentDictionary<string, object>();

        public BaseController(string domain, IMediator mediator) {
            _domain = domain;
            _mediator = mediator;
        }

        public ActionResult<TResponse> Submit<TResponse>(int id, IRequest<TResponse> request)
        {
            var @object = _dictionary.GetOrAdd($"{_domain}{id}", new object());

            lock (@object)
                return _mediator.Send(request).GetAwaiter().GetResult();
        }
    }
}
