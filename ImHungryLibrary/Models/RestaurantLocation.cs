using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class RestaurantLocation
    {
        [Key]
        public long Id { get; private set; }

        [MaxLength(25)]
        public string Province { get; set; }

        [MaxLength(35)]
        public string District { get; set; }

        [MaxLength(35)]
        public string Neighbourhood { get; set; }

        [MaxLength(15)]
        public string Street { get; set; }

        [MaxLength(10)]
        public string BuildingNo { get; set; }

        [MaxLength(10)]
        public string? ApartmentNo { get; set; }

        [MaxLength(55)]
        public string? Addition { get; set; }
    }
}
