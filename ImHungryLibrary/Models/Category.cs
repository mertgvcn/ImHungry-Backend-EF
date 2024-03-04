using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class Category
    {
        [Key]
        public long Id { get; private set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public long RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
