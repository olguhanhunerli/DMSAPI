using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.ComplaintDTO
{
    public class UpdateComplaintDTO
    {
        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public byte ChannelId { get; set; }

        [Required]
        public short TypeId { get; set; }

        [Required]
        public byte SeverityId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public bool IsRepeat { get; set; }

        public bool InterimActionRequired { get; set; }
        public string? InterimActionNote { get; set; }

        public int? AssignedTo { get; set; }
    }
}
