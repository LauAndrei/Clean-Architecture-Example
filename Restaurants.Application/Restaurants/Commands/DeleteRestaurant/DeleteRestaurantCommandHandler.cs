using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

public class DeleteRestaurantCommandHandler(
    ILogger<DeleteRestaurantCommand> logger,
    IRestaurantRepository restaurantRepository) : IRequestHandler<DeleteRestaurantCommand, bool>
{
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting restaurant with id: {RestaurantId}", request.Id);

        var restaurant = await restaurantRepository.GetByIdAsync(request.Id);

        if (restaurant is null)
        {
            return false;
        }

        return await restaurantRepository.Delete(restaurant);
    }
}