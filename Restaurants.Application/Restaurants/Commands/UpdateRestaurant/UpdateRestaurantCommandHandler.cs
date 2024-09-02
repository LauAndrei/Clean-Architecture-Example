using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger<UpdateRestaurantCommandHandler> logger,
    IRestaurantRepository restaurantRepository,
    IMapper mapper) : IRequestHandler<UpdateRestaurantCommand, bool>
{
    public async Task<bool> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with id: {RequestId} with {@UpdatedRestaurant}", request.Id,
            request);

        var restaurant = await restaurantRepository.GetByIdAsync(request.Id);

        if (restaurant is null)
        {
            return false;
        }

        mapper.Map(request, restaurant);

        return await restaurantRepository.SaveChanges() > 0;
    }
}