using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class Ingredient
    {
        [Key]
        public long Id { get; private set; }

        [MaxLength(30)]
        public string Name { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
