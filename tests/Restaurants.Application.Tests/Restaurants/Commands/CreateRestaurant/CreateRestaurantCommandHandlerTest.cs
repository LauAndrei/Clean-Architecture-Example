using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant;

[TestSubject(typeof(CreateRestaurantCommandHandler))]
public class CreateRestaurantCommandHandlerTest
{
    [Fact]
    public async Task Handle_ForValidCommands_ReturnsCreatedRestaurantId()
    {
        // arrange
        const int newlyCreatedRestaurantId = 43;
        const string ownerId = "owner-id";
        var command = new CreateRestaurantCommand();
        var restaurant = new Restaurant();

        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(m => m.Map<Restaurant>(command))
            .Returns(restaurant);

        var restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        restaurantRepositoryMock.Setup(repo => repo.Create(It.IsAny<Restaurant>()))
            .ReturnsAsync(newlyCreatedRestaurantId);

        var currentUser = new CurrentUser(ownerId, "test@gmail.com", [], null, null);
        var userContextMock = new Mock<IUserContext>();
        userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

        var commandHandler = new CreateRestaurantCommandHandler(loggerMock.Object,
            mapperMock.Object,
            restaurantRepositoryMock.Object,
            userContextMock.Object
        );

        // act
        var result = await commandHandler.Handle(command, CancellationToken.None);

        // assert
        result.Should().Be(newlyCreatedRestaurantId);
        restaurant.OwnerId.Should().Be(ownerId);
        restaurantRepositoryMock.Verify(r => r.Create(restaurant), Times.Once);
    }
}