using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.AuditLogs
{
    [ApiController]
    [Route("api/auditLogs")]
    public class AuditLogsController: BaseController
    {
        public AuditLogsController( IMediator mediator) 
            : base("AuditLog",mediator) { }

        [HttpPost]
        public ActionResult<SaveAuditLogCommand.Response> Add(SaveAuditLogCommand.Request request)
            => Submit(request.AuditLog.AuditLogId, request);

        [HttpGet("{domain}/{id}/{userId}/{fromAuditDate}/{toAuditDate}")]
        public async Task<ActionResult<GetAuditLogsQuery.Response>> Get([FromRoute]GetAuditLogsQuery.Request request)
            => await _mediator.Send(request);
    }
}
