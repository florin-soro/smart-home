using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Thermostat.Api.Middlewares
{
    public class FunctionAppRoleHandler : AuthorizationHandler<RolesAuthorizationRequirement>
    {
        private readonly ILogger<FunctionAppRoleHandler> logger;

        public FunctionAppRoleHandler(ILogger<FunctionAppRoleHandler> logger)
        {
            this.logger = logger;
        }
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RolesAuthorizationRequirement requirement)
        {
            foreach (var item in context.User.Claims)
            {
                logger.LogInformation("Claim: {Type} - {Value}", item.Type, item.Value);
            }

            if (context.User.Claims.Any(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" && c.Value == "access_as_function"))
            {
                context.Succeed(requirement);
            }
            else
            {
                foreach (var role in requirement.AllowedRoles)
                {
                    if (context.User.IsInRole(role))
                    {
                        context.Succeed(requirement);
                        break;
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
