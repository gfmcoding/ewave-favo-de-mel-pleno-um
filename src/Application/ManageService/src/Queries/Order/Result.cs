using System.Text.Json.Serialization;

namespace FavoDeMel.Application.ManageService.Queries.Order
{
    public class Result
    {
        [JsonPropertyName("id")]
        public long Id { get;  set; }
        [JsonPropertyName("position")]
        public int Position { get; set; }
        [JsonPropertyName("name")]
        public string Name { get;  set; }
        [JsonPropertyName("description")]
        public string Description { get;  set; }
        [JsonPropertyName("status")]
        public int Status { get;  set; }
        [JsonPropertyName("order_tab_id")]
        public long OrderTabId { get;  set; }
    }
}