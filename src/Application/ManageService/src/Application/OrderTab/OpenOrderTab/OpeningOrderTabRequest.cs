using System.Text.Json.Serialization;

namespace FavoDeMel.Application.ManageService.Application.OrderTab.OpenOrderTab
{
    public class OpeningOrderTabRequest
    {
        [JsonPropertyName("table_number")]
        public int TableNumber { get; set; }
    }
}