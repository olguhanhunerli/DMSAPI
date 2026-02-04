using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.CAPADTO
{
    public class CreateCapaDefaultsDTO
    {
        public string? ComplaintNo { get; set; }   
        public int CompanyId { get; set; }   
        public string? CompanyName { get; set; }
        public int OwnerId { get; set; }        
        public string OwnerName { get; set; }   
        public DateTime? DueDate { get; set; }    
        public string Status { get; set; } = "BEKLİYOR";
    }
}
