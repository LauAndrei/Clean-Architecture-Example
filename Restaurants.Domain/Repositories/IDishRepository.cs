using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishRepository
{
    Task<int> Create(Dish dish);

    Task Delete(IEnumerable<Dish> dishes);
}