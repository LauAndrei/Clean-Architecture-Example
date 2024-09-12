using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class MinimumAgeRequirementHandler(
    ILogger<MinimumAgeRequirementHandler> logger,
    IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        if (currentUser?.DateOfBirth is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        logger.LogInformation("User: {Email}, date of birth {DoB} - handling MinimumAgeRequirement",
            currentUser.Email, currentUser.DateOfBirth);

        if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) > DateOnly.FromDateTime(DateTime.Today))
        {
            logger.LogInformation("Authorization Failed");
            context.Fail();
            return Task.CompletedTask;
        }

        logger.LogInformation("Authorization Succeeded");
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}