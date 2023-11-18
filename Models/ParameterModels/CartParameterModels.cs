using WebAPI_Giris.Services.ControllerServices.Interfaces;

namespace ImHungryBackendER.Models.ParameterModels
{
    public class CartTransactionRequest
    {
        public long cartItemID { get; set; }
        public int itemID { get; set; }
        public int restaurantID { get; set; }
        public string? ingredients { get; set; }
        public int amount { get; set; }
    }
}
