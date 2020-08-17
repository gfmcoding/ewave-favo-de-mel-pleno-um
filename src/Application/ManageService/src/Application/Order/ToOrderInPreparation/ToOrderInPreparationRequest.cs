using System.Text.Json.Serialization;

namespace FavoDeMel.Application.ManageService.Application.Order.ToOrderInPreparation
{
    public class ToOrderInPreparationRequest
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}