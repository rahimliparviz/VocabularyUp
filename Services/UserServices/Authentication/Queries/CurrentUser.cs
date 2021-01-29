using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Errors;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Services.DTO;

namespace Services.UserServices.Authentication.Queries
{
    public class CurrentUser
    {
        public class Query : IRequest<AuthUser> { }

        public class Handler : IRequestHandler<Query, AuthUser>
        {
            private readonly UserManager<User> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            public Handler(UserManager<User> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
            }

            public async Task<AuthUser> Handle(Query request, CancellationToken cancellationToken)
            {
                var userName = _userAccessor.GetCurrentUsername() ??
                               throw new RestException(HttpStatusCode.Unauthorized);
                var user =  await _userManager.FindByNameAsync(userName);
                
                return new AuthUser
                {
                    Username = user.UserName,
                    Token = _jwtGenerator.CreateToken(user),
                };
            }
        }
    }
}