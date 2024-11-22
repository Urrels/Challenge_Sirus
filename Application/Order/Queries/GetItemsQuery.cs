using Application.Common;
using Domain.Entities;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Items.Queries;

public record GetItemsQuery() : IRequest<Response<List<Item>>>;

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, Response<List<Item>>>
{
    private readonly string _filePath;

    public GetItemsQueryHandler()
    {
        _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "items.json");
    }

    public async Task<Response<List<Item>>> Handle(GetItemsQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<List<Item>>();

        try
        {
            if (!File.Exists(_filePath))
            {
                response.ErrorProvider.AddError("File", "Items file not found.");
                return response;
            }

            var jsonData = await File.ReadAllTextAsync(_filePath, cancellationToken);

            var jsonObject = JsonSerializer.Deserialize<ItemsFile>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true 
            });

            if (jsonObject?.Articulos == null)
            {
                response.ErrorProvider.AddError("Deserialization", "Invalid JSON structure.");
                return response;
            }

            var validItems = jsonObject.Articulos
                .Where(item =>
                {
                    bool isValid = true;

                    if (item.Deposit != 1)
                    {
                        Console.WriteLine($"Item '{item.Code}' descartado: No está en el depósito 1.");
                        isValid = false;
                    }

                    if (item.Price <= 0)
                    {
                        Console.WriteLine($"Item '{item.Code}' descartado: Precio inválido ({item.Price}).");
                        isValid = false;
                    }

                    if (!IsValidDescription(item.Description))
                    {
                        Console.WriteLine($"Item '{item.Code}' descartado: Descripción inválida ('{item.Description}').");
                        isValid = false;
                    }

                    return isValid;
                })
                .ToList();

            response.Result = validItems;
        }
        catch (Exception ex)
        {
            response.ErrorProvider.AddError("Exception", ex.Message);
        }

        return response;
    }

    private bool IsValidDescription(string descripcion)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(descripcion ?? string.Empty, @"^[a-zA-Z0-9\s]+$");
    }

    private class ItemsFile
    {
        [JsonPropertyName("articulos")]
        public List<Item> Articulos { get; set; } = new();
    }
}
