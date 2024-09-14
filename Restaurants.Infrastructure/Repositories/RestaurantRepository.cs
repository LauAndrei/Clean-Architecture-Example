using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
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

    public async Task<(IEnumerable<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(string? searchPhrase,
        int pageNumber, int pageSize, string? sortBy, SortDirection sortDirection)
    {
        var searchPhraseLow = searchPhrase?.ToLower() ?? "";

        var baseQuery = dbContext.Restaurants
            .Where(r => r.Name.ToLower().Contains(searchPhraseLow)
                        || r.Description.ToLower().Contains(searchPhraseLow));

        var totalCount = await baseQuery.CountAsync();

        if (sortBy is not null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category },
            };

            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(columnsSelector[sortBy])
                : baseQuery.OrderByDescending(columnsSelector[sortBy]);
        }

        var restaurants = await baseQuery.Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (restaurants, totalCount);
    }
}