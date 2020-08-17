using System.Text.Json.Serialization;

namespace FavoDeMel.Application.ManageService.Application.Order.PlaceNewOrder
{
    public class PlaceNewOrderRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("order_tab_id")]
        public long OrderTabId { get; set; }
    }
}