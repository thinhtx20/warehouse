using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_manager.dto.Request
{
	public class CreatedMaterialRequestModel
	{
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal? Units { get; set; }
		public int Quantity { get; set; } = 0;
		public int CategoryId { get; set; } = 0;
	}
	public class UpdateMaterialRequestModel
	{
		public int Id { get; set; }
		public int CategoryId { get; set; }
		public string? Description { get; set; }
		public string Name { get; set; }
		public int Quantity { get; set; }
		public int Units { get; set; }
	}

}
