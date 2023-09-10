using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.Entities.Domain
{
	public abstract class BaseEntity
	{
		[Key]
		public int Id { get; set; }
		public Guid? Uid { get; set; } = Guid.NewGuid();

		[Required]
		public int CreatedBy { get; set; }

		public DateTime CreatedAt { get; set; }

		public int? UpdatedBy { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public byte StatusId { get; set; }
	}
}
