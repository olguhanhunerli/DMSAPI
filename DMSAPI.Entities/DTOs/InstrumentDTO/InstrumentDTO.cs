using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.InstrumentDTO
{
    public class InstrumentDTO
    {
        public ulong Instrument_Id { get; set; }

        public int CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;

        public string Asset_Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Serial_No { get; set; }

        public string? Measurement_Range { get; set; }
        public string? Resolution { get; set; }
        public string? Unit { get; set; }

        public string? Instrument_Type { get; set; }
        public string? Measurement_Discipline { get; set; }
        public bool Is_Critical { get; set; }
        public string? Risk_Level { get; set; }
        public string? Measurement_Uncertainty { get; set; }
        public bool Environment_Required { get; set; }
        public string? Environment_Notes { get; set; }

        public string Status { get; set; } = null!;

        public string? Location { get; set; }
        public string? Owner_Person { get; set; }

        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; } = null!;

        public int UpdatedBy { get; set; }
        public string UpdatedByName { get; set; } = null!;

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public int? DeletedBy { get; set; }
        public string? DeletedByUser { get; set; }

        public DateTime DeletedAt { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

    }
}
