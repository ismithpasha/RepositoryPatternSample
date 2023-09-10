using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternSample.Entities.Domain
{
	public class Menu : BaseEntity
	{
		public Menu()
		{
			RoleMenuPermission = new List<RoleMenuPermission>();
		}

		[Required, StringLength(100)]
		public string Name { get; set; }

		[Required, StringLength(200)]
		public string Code { get; set; }

		[StringLength(1000)]
		public string? Url { get; set; }

		public short? SortId { get; set; } = 0;

		[StringLength(200)]
		public string? MenuIcon { get; set; }

		public bool IsModule { get; set; }

		public bool IsParent { get; set; }

		public int? ParentId { get; set; }

		public bool? ShowMenu { get; set; }

		public bool? IsVisible { get; set; }

		public byte? LevelAt { get; set; }


		public virtual List<RoleMenuPermission> RoleMenuPermission { get; set; }

	}
}
