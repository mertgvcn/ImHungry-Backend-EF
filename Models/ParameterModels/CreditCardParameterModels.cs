namespace ImHungryBackendER.Models.ParameterModels
{
    public class AddCreditCardRequest
    {
        public string CreditCardNumber { get; set; }
        public string CreditCardHolderName { get; set; }
        public string ExpirationDate { get; set; }
        public short CVV { get; set; }
    }
}
