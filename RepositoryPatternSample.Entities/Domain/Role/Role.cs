using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.Entities.Domain
{
	public class Role : IdentityRole<int>
	{
		[StringLength(500)]
		public string? Description { get; set; }

		public DateTime CreatedAt { get; set; }

		public int? CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }

		public int? UpdatedBy { get; set; }

		public byte StatusId { get; set; }

		public virtual List<RoleMenuPermission>? UserMenuPermission { get; set; }
	}
}
