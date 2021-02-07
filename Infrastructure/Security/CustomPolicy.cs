using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Security
{
    public class CustomPolicy: IAuthorizationRequirement
    {
        
    }

    public class CustomPolicyHandler : AuthorizationHandler<CustomPolicy>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomPolicy requirement)
        {
            // context.Succeed(requirement);
            context.Fail();
            return Task.FromResult(0);
        }
    }
}