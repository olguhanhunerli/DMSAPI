using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class RootCauseMethod
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string NameTr { get; set; } = null!;
        public string NameEn { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
