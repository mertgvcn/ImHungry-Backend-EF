using System.ComponentModel.DataAnnotations;
using WebAPI_Giris.Models;

namespace ImHungryBackendER.Models.ViewModels
{
    public class ItemViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageSource { get; set; }

        public double Price { get; set; }

        public Category Category { get; set; }
    }
}
