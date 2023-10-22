namespace WebAPI_Giris.Models.Parameters.CreditCardParams
{
    public class AddCreditCardRequest
    {
        public string creditCardNumber { get; set; }
        public string creditCardHolderName { get; set; }
        public string expirationDate { get; set; }
        public int cvv { get; set; }
    }
}
