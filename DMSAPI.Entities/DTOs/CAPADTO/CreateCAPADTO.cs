using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CAPADTO
{
    public class CreateCAPADTO
    {

        public string? ComplaintNo { get; set; }

        [Required]
        public string Nonconformity { get; set; } = null!;

        [Required]
        public int RootCauseMethodId { get; set; }

        public string? RootCause { get; set; }
        public string? CorrectiveAction { get; set; }

        public string? Status { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public DateTime? DueDate { get; set; }

        public string? EffectivenessCheck { get; set; }
        public int? EffectivenessCheckedBy { get; set; }
    }
}
