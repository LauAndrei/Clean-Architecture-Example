using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirements;

public record HasMultipleRestaurantsRequirement(int MinimumNumber = 2) : IAuthorizationRequirement;