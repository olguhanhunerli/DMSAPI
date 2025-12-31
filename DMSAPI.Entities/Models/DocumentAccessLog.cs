using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class DocumentAccessLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DocumentId { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        [StringLength(50)]
        public string? AccessType { get; set; }  

        public DateTime? AccessAt { get; set; }

        [StringLength(50)]
        public string? IpAddress { get; set; }

        public Document Document { get; set; }
    }
}
