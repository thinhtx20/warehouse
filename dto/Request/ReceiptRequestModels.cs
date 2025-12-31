using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_manager.dto.Request
{
	public class ReceiptRequestModels
	{
		public int WarehouseId { get; set; }
		public int CreatedBy { get; set; }
		public string? Desciptions { get; set; }
		public DateTime? CreatedAt { get; set; }
		public List<ReceiptItem> Items { get; set; } = new List<ReceiptItem>();
	}
	public class ReceiptItem
	{
		public int MaterialId { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
	}
	public class ReceiptUpdateRequestModels
	{
		public int RececiptId { get; set; }
		public int WarehouseId { get; set; }
		public List<ReceiptItem> Items { get; set; } = new List<ReceiptItem>();
	}
	public class IssueRequestModels
	{
		public int WarehouseId { get; set; }

		public int CreatedBy { get; set; }

		public string? Desciptions { get; set; }
		public List<IssueItemRequest> Items { get; set; } = new();
	}

	public class IssueItemRequest
	{
		public int MaterialId { get; set; }

		public decimal Quantity { get; set; }

		public decimal UnitPrice { get; set; }
	}
	public class IssueUpdateRequestModels
	{
		public int IssueId { get; set; }

		public int WarehouseId { get; set; }

		public List<IssueItemUpdateRequest> Items { get; set; } = new();
	}

	public class IssueItemUpdateRequest
	{
		public int MaterialId { get; set; }

		public decimal Quantity { get; set; }

		public decimal UnitPrice { get; set; }
	}

}
