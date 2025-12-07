using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.RoleDTOs
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByUser { get; set; }
        public int? UploadedBy { get; set; }
        public string? UploadedByUser { get; set; }

       
    }
}
