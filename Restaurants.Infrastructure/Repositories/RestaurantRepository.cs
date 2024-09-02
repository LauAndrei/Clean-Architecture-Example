using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantRepository(RestaurantDbContext dbContext) : IRestaurantRepository
{
    public async Task<List<Restaurant>> GetAllAsync()
    {
        return await dbContext.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        return await dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<int> Create(Restaurant restaurant)
    {
        dbContext.Restaurants.Add(restaurant);
        await dbContext.SaveChangesAsync();

        return restaurant.Id;
    }

    public async Task<bool> Delete(Restaurant restaurant)
    {
        dbContext.Restaurants.Remove(restaurant);

        return await dbContext.SaveChangesAsync() > 0;
    }

    public async Task<int> SaveChanges()
    {
        return await dbContext.SaveChangesAsync();
    }
}