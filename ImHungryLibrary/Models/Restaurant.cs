using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class Restaurant
    {
        [Key]
        public long Id { get; private set; }

        public bool IsActive { get; set; } = true;
             
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(50)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? ImageSource { get; set; }

        public ICollection<Item> Items { get; set; }

        public RestaurantLocation Location { get; set; }

        public long? OwnerId { get; set; }
        public User? Owner { get; set; } = default!;
    }
}
