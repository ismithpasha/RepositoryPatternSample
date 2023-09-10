using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.Entities.Domain
{
	public class UserType : BaseEntity
	{ 
		[MaxLength(100)]
		[Required]
		public string Name { get; set; }
		[MaxLength(999)]
		public string? Description { get; set; }
		 
	}
}
