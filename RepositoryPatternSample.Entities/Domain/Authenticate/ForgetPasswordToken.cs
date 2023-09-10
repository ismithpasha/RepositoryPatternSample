using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternSample.Entities.Domain
{
	public class ForgetPasswordToken
	{
		public long Id { get; set; }
		[MaxLength(100)]
		public string Email { get; set; }
		[MaxLength(255)]
		public string Token { get; set; }

		public DateTime CreateOn { get; set; }
		public bool IsValid { get; set; }
		public DateTime ExpairOn { get; set; }
		public int? AttemptedCount { get; set; }
		public bool IsUsed { get; set; }
		public DateTime? UsedOn { get; set; }
	}
}
