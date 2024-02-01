using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImHungryLibrary.Models
{
    public class AuditLog
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Object { get; set; }
        [MaxLength(200)]
        public string Mutation { get; set; }
        public DateTime TimeStamp { get; } = DateTime.Now;
    }
}
