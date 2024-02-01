using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class Category
    {
        [Key]
        public long Id { get; private set; }

        [MaxLength(50)]
        public string Name { get; set; }
    }
}
