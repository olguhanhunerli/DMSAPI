using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CAPADTO
{
    public class CapaCreateLookupsDTO
    {
        public List<LookupItemDTO> RootCauseMethods { get; set; } = new();
        public List<LookupItemDTO> Owners { get; set; } = new(); // isteğe bağlı
    }
}
