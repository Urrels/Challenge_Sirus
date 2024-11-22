using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Seller
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("descripcion")]
        public string? Description { get; set; }

    }
}
