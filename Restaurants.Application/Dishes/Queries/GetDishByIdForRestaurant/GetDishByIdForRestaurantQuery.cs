using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;

public record GetDishByIdForRestaurantQuery(int RestaurantId, int DishId) : IRequest<DishDto>;