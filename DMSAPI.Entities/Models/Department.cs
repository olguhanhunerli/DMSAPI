using DMSAPI.Entities.Models;
using DMSAPI.Entities.Owned;
using System.ComponentModel.DataAnnotations;
using System;

public class Department : ICompanyOwned
{
	public int Id { get; set; }

	[Required]
	public string Name { get; set; }

	public string? DepartmentCode { get; set; }
	public string? Description { get; set; }
	public string? Notes { get; set; }

	public bool? IsActive { get; set; } = true;
	public int? SortOrder { get; set; }

	public int? ManagerId { get; set; }
	public User? Manager { get; set; }  

	public int CompanyId { get; set; }
	public Company Company { get; set; }

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

	public int? CreatedBy { get; set; }
	public User? CreatedByUser { get; set; }

	public int? UploadedBy { get; set; }
	public User? UploadedByUser { get; set; } 

	public bool IsDeleted { get; set; }

	public ICollection<User> Users { get; set; } = new List<User>();
}
