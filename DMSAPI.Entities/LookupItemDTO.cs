using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities
{
    public class LookupItemDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public string? Code { get; set; }
    }
}
