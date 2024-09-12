using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Users;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public class HasMultipleRestaurantsRequirementHandler(
    IUserContext userContext,
    IRestaurantRepository restaurantRepository) : AuthorizationHandler<HasMultipleRestaurantsRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        HasMultipleRestaurantsRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        var isAllowed = (await restaurantRepository.GetAllAsync())
            .Count(r => r.OwnerId == currentUser!.Id) >= requirement.MinimumNumber;

        if (isAllowed)
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}