using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.InstrumentDTO
{
    public class CreateInstrumentDTO
    {
        public int CompanyId { get; set; }

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

        public string? Location { get; set; }
        public string? Owner_Person { get; set; }
    }
}
