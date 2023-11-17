using System.ComponentModel.DataAnnotations;

namespace ImHungryBackendER.Models.ViewModels
{
    public class LocationViewModel
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        public string Neighbourhood { get; set; }

        public string? Street { get; set; }

        public string? BuildingNo { get; set; }

        public string? BuildingAddition { get; set; }

        public string? ApartmentNo { get; set; }

        public string? Note { get; set; }
    }
}
