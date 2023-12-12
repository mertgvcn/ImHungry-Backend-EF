namespace ImHungryBackendER.Models.ViewModels
{
    public class CartItemViewModel
    {
        public long Id { get; set; }

        public string? IngredientList { get; set; }

        public int Amount { get; set; }

        public CartItem_RestaurantViewModel Restaurant { get; set; }

        public CartItem_ItemViewModel Item { get; set; }
    }

    public class CartItem_RestaurantViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class CartItem_ItemViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string? ImageSource { get; set; }

        public double Price { get; set; }
    }
}
