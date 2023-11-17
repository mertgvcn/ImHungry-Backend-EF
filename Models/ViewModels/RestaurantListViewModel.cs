using System.ComponentModel.DataAnnotations;
using WebAPI_Giris.Models;

namespace ImHungryBackendER.Models.ViewModels
{
    public class RestaurantListViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string? Description { get; set; }

        public string? ImageSource { get; set; }

        public RestaurantLocation Location { get; set; }
    }
}
