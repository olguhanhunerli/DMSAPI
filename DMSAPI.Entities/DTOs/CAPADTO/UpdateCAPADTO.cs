using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CAPADTO
{
    public class UpdateCAPADTO
    {
        public string? Nonconformity { get; set; }
        public int? RootCauseMethodId { get; set; }
        public string? RootCause { get; set; }
        public string? CorrectiveAction { get; set; }
        public DateTime? DueDate { get; set; }
        public int? OwnerId { get; set; }
        public string? Status { get; set; }
        public string? EffectivenessCheck { get; set; }
        public int? EffectivenessCheckedBy { get; set; }
        public DateTime? EffectivenessCheckedAt { get; set; }
        public string? EffectivenessResult { get; set; }
    }
}
