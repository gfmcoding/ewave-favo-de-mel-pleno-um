using System.Text.Json.Serialization;

namespace FavoDeMel.Application.ManageService.Application.Order.ToDoneOrder
{
    public class ToDoneOrderRequest
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}