using System.Text.Json.Serialization;

namespace FavoDeMel.Application.ManageService.Application.Order.ReprioritizeOrder
{
    public class ReprioritizeOrderRequest
    {
        [JsonPropertyName("position")]
        public int Position { get; set; }
        [JsonPropertyName("new_position")]
        public int NewPosition { get; set; }
    }
}