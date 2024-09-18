using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.Requirements;
using Xunit;

namespace Restaurants.Infrastructure.Tests.Authorization.Requirements;

[TestSubject(typeof(HasMultipleRestaurantsRequirementHandler))]
public class HasMultipleRestaurantsRequirementHandlerTest
{
    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@gmail.com", [], default, default);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "4214"
            }
        };

        var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        restaurantRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new HasMultipleRestaurantsRequirement();
        var handler = new HasMultipleRestaurantsRequirementHandler(userContextMock.Object,
            restaurantRepositoryMock.Object);

        var authorizationHandlerContext = new AuthorizationHandlerContext([requirement], default!, null);

        // act
        await handler.HandleAsync(authorizationHandlerContext);

        // assert
        authorizationHandlerContext.HasSucceeded.Should().BeTrue();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldFail()
    {
        // arrange
        var currentUser = new CurrentUser("1", "test@gmail.com", [], default, default);

        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new()
            {
                OwnerId = currentUser.Id
            },
            new()
            {
                OwnerId = "4214"
            }
        };

        var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        restaurantRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new HasMultipleRestaurantsRequirement();
        var handler = new HasMultipleRestaurantsRequirementHandler(userContextMock.Object,
            restaurantRepositoryMock.Object);

        var authorizationHandlerContext = new AuthorizationHandlerContext([requirement], default!, null);

        // act
        await handler.HandleAsync(authorizationHandlerContext);

        // assert
        authorizationHandlerContext.HasSucceeded.Should().BeFalse();
    }
}