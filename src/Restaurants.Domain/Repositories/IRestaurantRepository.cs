using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantRepository
{
    Task<List<Restaurant>> GetAllAsync();

    Task<(IEnumerable<Restaurant> restaurants, int totalCount)> GetAllMatchingAsync(string? searchPhrase,
        int pageNumber, int pageSize, string? sortBy, SortDirection sortDirection);

    Task<Restaurant?> GetByIdAsync(int id);
    Task<int> Create(Restaurant restaurant);
    Task<bool> Delete(Restaurant restaurant);
    Task<int> SaveChanges();
}