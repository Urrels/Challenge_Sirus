using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Item
    {
        [JsonPropertyName("codigo")]
        public string? Code { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Description { get; set; }

        [JsonPropertyName("precio")]
        public double Price { get; set; }

        [JsonPropertyName("deposito")]
        public double Deposit { get; set; }
    }
}
