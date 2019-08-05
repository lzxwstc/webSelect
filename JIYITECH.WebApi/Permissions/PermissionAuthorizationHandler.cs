using IdentityModel;
using JIYITECH.WebApi.Configs;
using JIYITECH.WebApi.Entities;
using JIYITECH.WebApi.Repositories;
using JIYITECH.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JIYITECH.WebApi
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        private readonly AppConfig appConfig;
        public PermissionAuthorizationHandler(IOptions<AppConfig> appConfig)
        {
            this.appConfig = appConfig.Value;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                if (context.User.IsInRole("admin"))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    var userIdClaim = context.User.FindFirst(_ => _.Type == JwtClaimTypes.Id);
                    if (userIdClaim != null)
                    {
                        using (var uow = new UnitOfWork(appConfig.SQLConnectionStrings))
                        {
                            if (uow.UserRepository.CheckPermission(int.Parse(userIdClaim.Value), requirement.Name))
                            {
                                context.Succeed(requirement);
                            }
                        }

                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}