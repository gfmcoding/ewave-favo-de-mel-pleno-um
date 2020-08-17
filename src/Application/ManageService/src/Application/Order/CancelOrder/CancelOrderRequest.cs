using System.Text.Json.Serialization;

namespace FavoDeMel.Application.ManageService.Application.Order.CancelOrder
{
    public class CancelOrderRequest
    {
        [JsonPropertyName("order_tab_id")]
        public long OrderTabId { get; set; }
        [JsonPropertyName("order_id")]
        public long OrderId { get; set; }
    }
}