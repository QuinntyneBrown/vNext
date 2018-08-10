using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Dapper;
using System;
using vNext.Core.Interfaces;
using vNext.Core.Models;
using vNext.Core.Interfaces;
using vNext.Core.Extensions;

namespace vNext.API.Features.AddressEmails
{
    public class RemoveAddressEmailCommand
    {
        public class Request : IRequest
        {
            public AddressEmailDto AddressEmail { get; set; }
        }

        public class Handler : IRequestHandler<Request>
        {
            private readonly ISqlConnectionManager _sqlConnectionManager;
            public Handler(ISqlConnectionManager sqlConnectionManager)
                => _sqlConnectionManager = sqlConnectionManager;

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                using (var connection = _sqlConnectionManager.GetConnection())
                {
                    await connection.ExecuteProcAsync("[Comsense].[ProcAddressEmailDelete]", new { request.AddressEmail.AddressEmailId });
                }
            }

        }
    }
}
