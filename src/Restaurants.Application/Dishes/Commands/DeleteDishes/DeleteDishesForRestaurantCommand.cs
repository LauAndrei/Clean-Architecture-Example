using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;

public record DeleteDishesForRestaurantCommand(int RestaurantId) : IRequest;