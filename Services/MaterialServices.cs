using Inventory_manager.dto.Response;
using Inventory_manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_manager.Services
{
	public class MaterialServices
	{
		private readonly WarehousesManagerContext _context;
		public MaterialServices()
		{
			_context = new WarehousesManagerContext();
		}
		public async Task<List<MaterialResponeMessage>> GetMaterialsAsync(List<int>? Ids)
		{
			var query = await _context.Materials.AsNoTracking().Include(x => x.Category).Where(x => x.IsActive == true)
				.Where(x => Ids == null || !Ids.Any() || Ids.Contains(x.MaterialId)) 
				.Select(x => new MaterialResponeMessage
				{
					CategoryName = x.Category.CategoryName,
					MaterialId = x.MaterialId,
					MaterialName = x.MaterialName,
					// Quantity = x.Quantity,
					Unit = x.Unit
				}).ToListAsync();
			return query;
		}
		public async Task<List<int>> GetMaterialInReceipt(int ReceiptId)
		{
			var lst = new List<int>();
			if (ReceiptId == null) return null;
			try
			{
				var lstItem = await _context.InventoryReceiptDetails.AsNoTracking()
					.Where(x => x.ReceiptId == ReceiptId).Select(x => x.MaterialId.Value).ToListAsync();
				lst = lstItem.ToList();
				return lst;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public async Task<List<int>> GetMaterialInIssue(int IssueId)
		{
			var lst = new List<int>();
			if (IssueId == null) return null;
			try
			{
				var lstItem = await _context.InventoryIssueDetails.AsNoTracking()
					.Where(x => x.IssueId == IssueId).Select(x => x.MaterialId.Value).ToListAsync();
				lst = lstItem.ToList();
				return lst;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
