using Application.Items.Commands;
using Application.Items.Queries;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetItems()
    {
        var result = await _mediator.Send(new GetItemsQuery());
        if (result.ErrorProvider.HasErrors)
            return BadRequest(result.ErrorProvider);

        return Ok(result.Result);
    }


    [HttpPost("save-order")]
    public async Task<IActionResult> SaveOrder([FromBody] Order order)
    {
        var command = new SaveOrderCommand(order);

        var result = await _mediator.Send(command);

        if (result.ErrorProvider.HasErrors)
            return BadRequest(result.ErrorProvider);
        return Ok(result.Result);
    }

}