using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using vNext.Core.DomainEvents;
using vNext.API.Features.Users;
using vNext.Core.Interfaces;
using vNext.API.Features.Contacts;

namespace vNext.API.ProcessManagers
{
    public class CreateUserProcess
        : INotificationHandler<UserCreationRequested>
    {
        private readonly IMediator _mediator;
        private readonly IProcedure<SaveUserCommand.Request, short> _saveUserProcedure;
        private readonly IProcedure<SaveContactCommand.Request, int> _saveContactProcedure;

        public CreateUserProcess(
            IMediator mediator,
            IProcedure<SaveUserCommand.Request, short> saveUserProcedure,
            IProcedure<SaveContactCommand.Request, int> saveContactProcedure
            )
        {
            _mediator = mediator;
            _saveUserProcedure = saveUserProcedure;
            _saveContactProcedure = saveContactProcedure;
        }
        public async Task Handle(UserCreationRequested notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
