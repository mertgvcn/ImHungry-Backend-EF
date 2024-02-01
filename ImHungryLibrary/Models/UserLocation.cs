using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class UserLocation
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(50)]
        public string Province { get; set; }

        [MaxLength(50)]
        public string District { get; set; }

        [MaxLength(50)]
        public string Neighbourhood { get; set; }

        [MaxLength(50)]
        public string? Street { get; set; }

        [MaxLength(10)]
        public string? BuildingNo { get; set; }

        [MaxLength(10)]
        public string? BuildingAddition { get; set; }

        [MaxLength(10)]
        public string? ApartmentNo { get; set; }

        [MaxLength(55)]
        public string? Note { get; set; }
    }
}
