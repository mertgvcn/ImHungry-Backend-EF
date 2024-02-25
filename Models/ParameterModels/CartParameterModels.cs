namespace ImHungryBackendER.Models.ParameterModels

{
    public class CartTransactionRequest
    {
        public long CartItemID { get; set; }
        public int ItemID { get; set; }
        public int RestaurantID { get; set; }
        public string? Ingredients { get; set; }
        public int Amount { get; set; }
    }
}
