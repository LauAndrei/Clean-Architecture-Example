using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;

namespace NET8_CleanArchitecture_Azure.Controllers;

[ApiController]
[Route("api/restaurants/{restaurantId:int}/[controller]")]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        
        await mediator.Send(command);

        return Created();
    }
}