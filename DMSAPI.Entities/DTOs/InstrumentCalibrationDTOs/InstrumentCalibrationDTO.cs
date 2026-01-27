using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.InstrumentCalibrationDTOs
{
    public class InstrumentCalibrationDTO
    {
        public ulong CalibrationId { get; set; }

        public int InstrumentId { get; set; }
        public string? AssetCode { get; set; }
        public string? InstrumentName { get; set; }
        public string? SerialNo { get; set; }
        public string? InstrumentLocation { get; set; }

        public DateTime CalibrationDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? IntervalMonths { get; set; }

        public string Result { get; set; } = default!;
        public string? CalibrationCompany { get; set; }
        public string? CertificateNo { get; set; }
        public string? Notes { get; set; }

        public string? CalibrationType { get; set; }
        public string? Standard { get; set; }
        public string? Traceability { get; set; }
        public string? ActionTaken { get; set; }
        public bool? RequiresRecalibration { get; set; }

        public int? ApprovedBy { get; set; }
        public string? ApprovedByName { get; set; }
        public DateTime? ApprovedAt { get; set; }

        public int? RemainingDays { get; set; }

        public int CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public int CreatedBy { get; set; }
        public string? CreatedByName { get; set; }

        public int? UpdatedBy { get; set; }
        public string? UpdatedByName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<InstrumentCalibrationFileDTO> Files { get; set; } = new();
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

    }
}
