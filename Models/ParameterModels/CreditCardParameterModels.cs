namespace ImHungryBackendER.Models.ParameterModels
{
    public class AddCreditCardRequest
    {
        public string creditCardNumber { get; set; }
        public string creditCardHolderName { get; set; }
        public string expirationDate { get; set; }
        public short cvv { get; set; }
    }
}
