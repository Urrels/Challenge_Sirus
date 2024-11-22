using Application.Common;
using Domain.Entities;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Sellers.Queries;

public record GetSellersQuery() : IRequest<Response<List<Seller>>>;

public class GetSellersQueryHandler : IRequestHandler<GetSellersQuery, Response<List<Seller>>>
{
    private readonly string _filePath;

    public GetSellersQueryHandler()
    {
        _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "sellers.json");
    }

    public async Task<Response<List<Seller>>> Handle(GetSellersQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<List<Seller>>();

        try
        {
            Console.WriteLine($"Checking if file exists: {_filePath}");

            if (!File.Exists(_filePath))
            {
                Console.WriteLine("File not found.");
                response.ErrorProvider.AddError("File", "Sellers file not found.");
                return response;
            }

            var jsonData = await File.ReadAllTextAsync(_filePath, cancellationToken);
            Console.WriteLine($"File content read successfully: {jsonData}");

            var jsonObject = JsonSerializer.Deserialize<SellersFile>(jsonData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (jsonObject?.Vendedores == null || !jsonObject.Vendedores.Any())
            {
                Console.WriteLine("Deserialization failed or no vendedores found.");
                response.ErrorProvider.AddError("Deserialization", "Invalid JSON structure or no vendedores available.");
                return response;
            }

            Console.WriteLine($"Number of vendedores found: {jsonObject.Vendedores.Count}");

            response.Result = jsonObject.Vendedores;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            response.ErrorProvider.AddError("Exception", ex.Message);
        }

        return response;
    }

    private class SellersFile
    {
        [JsonPropertyName("vendedores")] 
        public List<Seller> Vendedores { get; set; } = new();
    }
}
