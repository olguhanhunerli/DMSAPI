using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.DTOs.PermissionDTOs
{
    public class PermissionDTO
    {
        [DefaultValue(true)]
        public bool CanView { get; set; }

        [DefaultValue(true)]
        public bool CanUpload { get; set; }

        [DefaultValue(true)]
        public bool CanDownload { get; set; }

        [DefaultValue(false)]
        public bool CanShare { get; set; }

        [DefaultValue(false)]
        public bool CanDelete { get; set; }

        [DefaultValue(false)]
        public bool CanApprove { get; set; }
    }
}
