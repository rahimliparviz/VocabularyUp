using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Services.UserServices.Authentication.Commands
{
    public class Logout
    {
        public class Command : IRequest
        { }

    

        public class Handler : IRequestHandler<Command>
        {
            private readonly SignInManager<User> _signInManager;
            public Handler( SignInManager<User> signInManager)
            {
                _signInManager = signInManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                     await _signInManager.SignOutAsync();

                 return Unit.Value;

                
            }
        }
    }
}