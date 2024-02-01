using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class Item
    {
        [Key]
        public long Id { get; private set; }

        [MaxLength(30)]
        public string Name { get; set; } = default!; //default! alttaki yeşil boku götürmek için

        [MaxLength(55)]
        public string? Description { get; set;}

        [MaxLength(40)]
        public string? ImageSource { get; set; }

        public double Price { get; set; }

        public Restaurant Restaurant { get; set; }

        public Category Category { get; set; }

        public ICollection<Ingredient>? Ingredients { get; set;}
    }
}
