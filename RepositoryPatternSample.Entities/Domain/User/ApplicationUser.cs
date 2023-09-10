using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.Entities.Domain
{
	public class ApplicationUser : IdentityUser<int>
	{
		public ApplicationUser()
		{
			this.Tokens = new List<UserToken>();
		}

		public int? TypeId { get; set; }

		[Required]
		[MaxLength(100)]
		[StringLength(100)]
		public string? Name { get; set; }
		public string? Address { get; set; }
		public bool TempPass { get; set; }
		public byte StatusId { get; set; }

		public DateTime? CreatedAt { get; set; }
		public int? CreatedBy { get; set; }

		public DateTime? UpdatedAt { get; set; }
		public int? UpdatedBy { get; set; }

		public virtual List<UserToken>? Tokens { get; set; }
		[ForeignKey("TypeId")]
		public virtual UserType? UserType { get; set; }
	}
}
