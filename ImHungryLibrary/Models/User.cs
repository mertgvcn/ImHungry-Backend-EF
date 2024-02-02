using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string Username { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(150)]
        public string Password { get; set; }
        
        public ICollection<UserLocation> Locations { get; set; }

        public ICollection<CreditCard> CreditCards { get; set; }

        public ICollection<Cart> CartItems { get; set; }

        public ICollection<Role> Roles { get; set; }

        public long? CurrentLocationId { get; set; }
        public UserLocation? CurrentLocation { get; set; }

    }
}
