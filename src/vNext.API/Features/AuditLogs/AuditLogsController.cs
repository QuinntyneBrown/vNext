using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using vNext.Core.Interfaces;

namespace vNext.API.Features.AuditLogs
{
    [ApiController]
    [Route("api/auditLogs")]
    public class AuditLogsController: BaseController
    {
        public AuditLogsController(IDateTime dateTime, IHttpContextAccessor httpContextAccessor, IMediator mediator) 
            : base(dateTime,"AuditLog",httpContextAccessor, mediator) { }

        [HttpPost]
        public ActionResult<SaveAuditLogCommand.Response> Add(SaveAuditLogCommand.Request request)
            => Submit(request.AuditLog.AuditLogId, request);

        [HttpDelete("{auditLogId}/{concurrencyVersion}")]
        public ActionResult<RemoveAuditLogCommand.Response> Remove(RemoveAuditLogCommand.Request request)
            => Submit(request.AuditLogId, request);

        [HttpGet("{auditLogId}")]
        public async Task<ActionResult<GetAuditLogByIdQuery.Response>> GetById([FromRoute]GetAuditLogByIdQuery.Request request)
            => await _mediator.Send(request);

        [HttpGet]
        public async Task<ActionResult<GetAuditLogsQuery.Response>> Get()
            => await _mediator.Send(new GetAuditLogsQuery.Request());
    }
}
