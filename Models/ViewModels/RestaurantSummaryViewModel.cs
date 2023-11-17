namespace ImHungryBackendER.Models.ViewModels
{
    public class RestaurantSummaryViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string? Description { get; set; }

        public string? ImageSource { get; set; }
    }
}
