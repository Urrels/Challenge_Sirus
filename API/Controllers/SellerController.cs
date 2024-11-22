using Application.Sellers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/sellers")]
public class SellersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SellersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetSellers()
    {
        var result = await _mediator.Send(new GetSellersQuery());
        if (result.ErrorProvider.HasErrors)
            return BadRequest(result.ErrorProvider);

        return Ok(result.Result);
    }

}