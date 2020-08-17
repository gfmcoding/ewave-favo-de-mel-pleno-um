namespace FavoDeMel.Application.ManageService.Application.Order
{
    public enum OrderRequestTypes
    {
        PlaceNewOrder,
        ReprioritizeOrder,
        CancelOrder,
        ToOrderInPreparation,
        ToOrderInDone,
        ToOrderInDelivery
    }
}