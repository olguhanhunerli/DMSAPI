using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CAPADTO
{
    public   class ClosedCAPADTO
    {
        public string? ClosureEvidence { get; set; } 
        public string? EffectivenessResult { get; set; } 
        public string? EffectivenessCheck { get; set; }  
        public int? EffectivenessCheckedBy { get; set; } 
        public DateTime? EffectivenessCheckedAt { get; set; } 
    }
}
