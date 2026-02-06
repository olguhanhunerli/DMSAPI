using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CAPADTO
{
    public class CreateCAPAActionDTO
    {
        public string ActionType { get; set; } = null!;

        public string Description { get; set; } 

        public int OwnerId { get; set; }

        public DateTime? DueDate { get; set; }

        public bool EvidenceRequired { get; set; } = false;
    }
}
