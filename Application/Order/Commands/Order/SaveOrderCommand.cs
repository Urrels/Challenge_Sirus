using Application.Common;
using Domain.Entities;
using MediatR;

namespace Application.Items.Commands;

public record SaveOrderCommand(Order Order) : IRequest<Response<bool>>;

public class SaveOrderCommandHandler : IRequestHandler<SaveOrderCommand, Response<bool>>
{
    public Task<Response<bool>> Handle(SaveOrderCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<bool>();

        if (request.Order.Items == null || !request.Order.Items.Any())
        {
            response.ErrorProvider.AddError("Validation", "You must select at least one item.");
            return Task.FromResult(response);
        }

        foreach (var item in request.Order.Items)
        {
            if (item.Price <= 0)
                response.ErrorProvider.AddError("Validation", $"The price of the item {item.Code} is not valid.");

            if (!System.Text.RegularExpressions.Regex.IsMatch(item.Description ?? "", @"^[a-zA-Z0-9\s]+$"))
                response.ErrorProvider.AddError("Validation", $"The description of the item {item.Code} contains special characters.");
        }

        if (response.ErrorProvider.HasErrors)
            return Task.FromResult(response);

        response.Result = true; 
        return Task.FromResult(response);
    }
}
