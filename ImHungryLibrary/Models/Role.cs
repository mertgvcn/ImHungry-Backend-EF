using System.ComponentModel.DataAnnotations;

namespace ImHungryLibrary.Models
{
    public class Role
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(50)]
        public string RoleName { get; set; }

        public ICollection<User>? Users { get; set; }
    }
}
