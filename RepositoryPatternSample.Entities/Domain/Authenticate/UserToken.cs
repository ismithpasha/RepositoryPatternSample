using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.Entities.Domain
{
	public class UserToken
	{
		[Key]
		public int UserTokenId { get; set; }

		[Required]
		public string? Token { get; set; }

		public string? RefreshToken { get; set; }

		public DateTime CreationDate { get; set; }
		public DateTime ExpirationDate { get; set; }

		public string? IpAddress { get; set; }

		public bool IsInvalidated { get; set; }

		public int UserId { get; set; }

		[NotMapped]
		public bool IsActive
		{
			get
			{
				return ExpirationDate > DateTime.UtcNow;
			}
		}


		[ForeignKey("UserId")]
		public virtual ApplicationUser? User { get; set; }


	}
}
