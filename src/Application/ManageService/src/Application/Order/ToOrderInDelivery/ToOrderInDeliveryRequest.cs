using System.Text.Json.Serialization;

namespace FavoDeMel.Application.ManageService.Application.Order.ToOrderInDelivery
{
    public class ToOrderInDeliveryRequest
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}