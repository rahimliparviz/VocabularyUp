using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.UserServices.Authentication.Commands;
using Services.UserServices.Authentication.Queries;

namespace Api.Controllers.User
{
    public class AuthController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthUser>> Login([FromBody] Login.Query query)
        {
            return await Mediator.Send(query);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthUser>> Register([FromBody] Register.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<AuthUser>> CurrentUser()
        {
            return await Mediator.Send(new CurrentUser.Query());
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<Unit> Logout()
        {
            return await Mediator.Send(new Logout.Command());
        }
    }
}