using MediatR;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQuery : IRequest<PagedResult<RestaurantDto>>
{
    public string? SearchPhrase { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public string? SortBy { get; init; }
    public SortDirection SortDirection { get; init; }
}