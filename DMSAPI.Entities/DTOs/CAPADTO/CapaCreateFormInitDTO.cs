using DMSAPI.Entities.DTOs.ComplaintDTO;
using DMSAPI.Entities.DTOs.CustomerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CAPADTO
{
    public class CapaCreateFormInitDTO
    {
        public CustomerMiniDTO Customer { get; set; } = null!;
        public ComplaintDTO.ComplaintDTO Complaint { get; set; } = null!;

        public CreateCapaDefaultsDTO Defaults { get; set; } = new();
        public CapaCreateLookupsDTO Lookups { get; set; } = new();

    }
}
