using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_manager.dto.Response
{
	public class MaterialResponeMessage
	{
		public int MaterialId { get; set; }
		public string MaterialName { get; set; }
		public decimal Unit { get; set; }
		public int Quantity { get; set; }
		public int QuantitySL { get; set; }
		public decimal? Weight { get; set; }
        //public int CategoryId { get; set; }
        public string CategoryName { get; set; }
	}
	public class WarehouseMaterialRespone
	{
		public int WarehouseId { get; set; }
		public string WarehouseName { get; set; }
	}
	public class ListReceiptResponeMessage
	{
		public int ReceiptID { get; set; }
		public string? WarehouseName { get; set; }
		public int? WarehouseID { get; set; }
		public string? WarehouseDescription { get; set; }
		public int TotalMaterial {  get; set; }=  0;
		public string? CreatedBy {  get; set; }
		public DateTime? CreatedAt {  get; set; }

	}
	public class ListIssueResponseMessage
	{
		public int IssueID { get; set; }

		public string IssueCode { get; set; }

		public string WarehouseName { get; set; }

		public int? WarehouseID { get; set; }

		public string? WarehouseDescription { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? CreatedAt { get; set; }

		public decimal TotalMaterial { get; set; }
	}
	public class MaterialCategoryRespone
	{
		public int? Id { get; set; }
		public string? Name { get; set; }
	}
	public class MaterialByIdResponeMessage
	{
		public int MaterialId { get; set; }
		public string? MaterialName { get; set; }
		public decimal Unit { get; set; }
		public int Quantity { get; set; }
		public int CategoryId { get; set; }
		public string? CategoryName { get; set; }
		public string? Description { get; set; }
	}
}
