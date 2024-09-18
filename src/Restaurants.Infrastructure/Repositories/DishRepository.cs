using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class DishRepository(RestaurantDbContext dbContext) : IDishRepository
{
    public async Task<int> Create(Dish dish)
    {
        dbContext.Dishes.Add(dish);
        await dbContext.SaveChangesAsync();

        return dish.Id;
    }

    public async Task Delete(IEnumerable<Dish> dishes)
    {
        dbContext.RemoveRange(dishes);
        await dbContext.SaveChangesAsync();
    }
}