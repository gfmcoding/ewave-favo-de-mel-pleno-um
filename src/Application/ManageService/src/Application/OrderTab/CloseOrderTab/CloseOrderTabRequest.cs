using System.Text.Json.Serialization;

namespace FavoDeMel.Application.ManageService.Application.OrderTab.CloseOrderTab
{
    public class CloseOrderTabRequest
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}