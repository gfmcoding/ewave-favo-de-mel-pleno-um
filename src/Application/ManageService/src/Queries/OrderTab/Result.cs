using System.Collections.Generic;
using System.Text.Json.Serialization;
using FavoDeMel.Application.ManageService.Core;

namespace FavoDeMel.Application.ManageService.Queries.OrderTab
{
    public class Result
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("table_number")]
        public int TableNumber { get; set; }
        [JsonPropertyName("is_closed")]
        public bool IsClosed { get; set; }
        [JsonPropertyName("orders")]
        public List<Core.Order> Orders { get; set; }
    }
}