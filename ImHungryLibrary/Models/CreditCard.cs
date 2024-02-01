using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class CreditCard
    {
        [Key]
        public long Id { get; private set; }

        [MaxLength(19)]
        public string Number { get; set; }

        [MaxLength(50)]
        public string HolderName { get; set; }

        [MaxLength(5)]
        public string ExpirationDate { get; set; }

        public short CVV { get; set; }
    }
}
