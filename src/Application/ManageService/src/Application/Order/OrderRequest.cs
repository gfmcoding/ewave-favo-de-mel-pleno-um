namespace FavoDeMel.Application.ManageService.Application.Order
{
    public class OrderRequest
    {
        public OrderRequestTypes Type { get; set; }
        public object Request { get; set; }
    }
}