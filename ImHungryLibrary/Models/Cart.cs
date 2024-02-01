using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class Cart
    {
        [Key]
        public long Id { get; private set; }

        [MaxLength(150)]
        public string? IngredientList { get; set; }

        public int Amount { get; set; }

        public long UserId { get; set; }

        public Restaurant Restaurant { get; set; }

        public Item Item { get; set; }
    }
}
