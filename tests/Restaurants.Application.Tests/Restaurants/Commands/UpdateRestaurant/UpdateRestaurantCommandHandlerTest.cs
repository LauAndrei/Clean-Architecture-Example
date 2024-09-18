using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

[TestSubject(typeof(UpdateRestaurantCommandHandler))]
public class UpdateRestaurantCommandHandlerTest
{
    private const int RestaurantId = 14;

    private readonly UpdateRestaurantCommand _command = new()
    {
        Id = RestaurantId,
        Name = "Name Updated",
        Description = "Description Updated",
        HasDelivery = false
    };

    private readonly Restaurant _foundRestaurant = new()
    {
        Id = RestaurantId,
        Name = "Restaurant Name",
        Description = "Restaurant Description",
        Category = "Fast Food",
        HasDelivery = true,
        ContactEmail = "restaurant@gmail.com",
        ContactNumber = "123456789",
        Address = null,
        Dishes = []
    };

    private readonly UpdateRestaurantCommandHandler _handler;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock;

    private readonly Mock<IRestaurantRepository> _restaurantRepositoryMock;

    private readonly Restaurant _updatedRestaurant = new()
    {
        Id = RestaurantId,
        Name = "Name Updated",
        Description = "Description Updated",
        Category = "Fast Food",
        HasDelivery = false,
        ContactEmail = "restaurant@gmail.com",
        ContactNumber = "123456789",
        Address = null,
        Dishes = []
    };

    public UpdateRestaurantCommandHandlerTest()
    {
        var loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        _restaurantRepositoryMock = new Mock<IRestaurantRepository>();
        _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationService>();
        _mapperMock = new Mock<IMapper>();

        _handler = new UpdateRestaurantCommandHandler(loggerMock.Object,
            _restaurantRepositoryMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ForValidUpdateRestaurant_ShouldBeSuccessful()
    {
        // arrange
        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(_command.Id)).ReturnsAsync(_foundRestaurant);

        _restaurantAuthorizationServiceMock.Setup(auth => auth.Authorize(_foundRestaurant, ResourceOperation.Update))
            .Returns(true);

        _mapperMock.Setup(m => m.Map<Restaurant>(_command))
            .Returns(_updatedRestaurant);

        // act 
        try
        {
            await _handler.Handle(_command, CancellationToken.None);
        }
        catch
        {
            // ignored
        }

        // assert
        _restaurantRepositoryMock.Verify(r => r.SaveChanges(), Times.Once);
        _mapperMock.Verify(m => m.Map(_command, _foundRestaurant), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // arrange
        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(RestaurantId)).ReturnsAsync(null as Restaurant);

        // act
        // ReSharper disable once SuggestVarOrType_Elsewhere
        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        // assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"{nameof(Restaurant)} with id: {RestaurantId} doesn't exist.");
    }

    [Fact]
    public async Task Handle_WithNonExistingRestaurant_ShouldThrowForbidException()
    {
        // arrange
        _restaurantRepositoryMock.Setup(r => r.GetByIdAsync(RestaurantId)).ReturnsAsync(_foundRestaurant);

        _restaurantAuthorizationServiceMock.Setup(auth => auth.Authorize(_foundRestaurant, ResourceOperation.Update))
            .Returns(false);

        // act
        // ReSharper disable once SuggestVarOrType_Elsewhere
        Func<Task> act = async () => await _handler.Handle(_command, CancellationToken.None);

        // assert
        await act.Should().ThrowAsync<ForbidException>();
    }
}