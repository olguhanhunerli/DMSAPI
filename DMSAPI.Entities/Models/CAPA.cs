using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class CAPA
    {
        [Key]
        public long Id { get; set; }

        public string CapaNo { get; set; } = null!;

        public string? ComplaintNo { get; set; }

        public string Nonconformity { get; set; } = null!;

        public int RootCauseMethodId { get; set; }
        public RootCauseMethod RootCauseMethod { get; set; } = null!;

        public string? RootCause { get; set; }
        public string? CorrectiveAction { get; set; }

        public string? Status { get; set; }

        public int OwnerId { get; set; }

        public int CompanyId { get; set; }  

        public bool IsClosed { get; set; }   

        public string? ClosureEvidence { get; set; }

        public string? EffectivenessCheck { get; set; }
        public int? EffectivenessCheckedBy { get; set; }

        public string? EffectivenessResult { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? EffectivenessCheckedAt { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Company? Company { get; set; }

        [ForeignKey(nameof(ComplaintNo))]
        public Complaint? Complaints { get; set; }

        public User? OwnerByUser { get; set; }
        public User? EffectivenessCheckedByUser { get; set; }

        public ICollection<CAPAACTION> Actions { get; set; } = new List<CAPAACTION>();

        public ICollection<CapaEvidenceFiles> EvidenceFiles { get; set; } = new List<CapaEvidenceFiles>();
	}
}
