using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace vNext.API.Features.Users
{
    [ApiController]
    [Route("api/users")]
    public class UsersController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator) => _mediator = mediator;
        
        [HttpPost("signin")]
        public async Task<ActionResult<AuthenticateCommand.Response>> SignIn(AuthenticateCommand.Request request)
            => await _mediator.Send(request);

        [HttpGet("signout")]
        public void SignOut() { }

        [HttpGet("")]
        public async Task<ActionResult<GetUsersQuery.Response>> GetAll()
            => await _mediator.Send(new GetUsersQuery.Request());

        [HttpPost("save")]
        public async Task<ActionResult<SaveUserCommand.Response>> Save(SaveUserCommand.Request request)
            => await _mediator.Send(request);

        [HttpPost("changePassword")]
        public async Task ChangePassword(UserChangePasswordCommand.Request request) 
            => await _mediator.Send(request);

        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserByIdQuery.Response>> GetById(GetUserByIdQuery.Request request)
            => await _mediator.Send(request);
    }
}
