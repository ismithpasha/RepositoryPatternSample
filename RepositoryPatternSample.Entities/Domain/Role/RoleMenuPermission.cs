using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.Entities.Domain
{
	public class RoleMenuPermission : BaseEntity
	{ 

		[Required]
		public int RoleId { get; set; }

		[Required]
		public int MenuId { get; set; }


		[ForeignKey("MenuId")]
		public virtual Menu? Menu { get; set; }

		[ForeignKey("RoleId")]
		public virtual Role? ApplicationRole { get; set; }
	}
}
