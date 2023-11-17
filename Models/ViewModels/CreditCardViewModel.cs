using System.ComponentModel.DataAnnotations;

namespace ImHungryBackendER.Models.ViewModels
{
    public class CreditCardViewModel
    {
        public long Id { get; set; }

        public string Number { get; set; }

        public string HolderName { get; set; }

        public string ExpirationDate { get; set; }

        public short CVV { get; set; }
    }
}
