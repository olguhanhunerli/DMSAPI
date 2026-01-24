using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Entities.Models
{
    public class Instrument
    {
		[Key]
		public int Instrument_Id { get; set; }
		public int CompanyId { get; set; }
		[ForeignKey(nameof(CompanyId))]
		public Company Company { get; set; }

		[Required, MaxLength(50)]
		public string Asset_Code { get; set; } = null!;

		[Required, MaxLength(120)]
		public string Name { get; set; } = null!;

		[MaxLength(80)]
		public string? Brand { get; set; }

		[MaxLength(80)]
		public string? Model { get; set; }

		[MaxLength(80)]
		public string? Serial_No { get; set; }

		[MaxLength(80)]
		public string? Measurement_Range { get; set; }

		[MaxLength(40)]
		public string? Resolution { get; set; }

		[MaxLength(20)]
		public string? Unit { get; set; }

		[MaxLength(120)]
		public string? Location { get; set; }
		public string Status { get; set; }
		[MaxLength(120)]
		public string? Owner_Person { get; set; }
		public int CreatedBy { get; set; }
		public User CreatedByName { get; set; }
		public int? UpdatedBy { get; set; }
		public User? UpdatedByName { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public int? DeletedBy { get; set; }
		public User? DeletedByUser { get; set; }
		public DateTime? DeletedAt { get; set; }
		public DateTime Created_At { get; set; }
		public DateTime Updated_At { get; set; }
	}
}
