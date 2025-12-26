using Azure.Core;
using Inventory_manager.dto.Request;
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
		public MaterialServices()
		{
		}
		public async Task<List<MaterialResponeMessage>> GetMaterialsAsync(List<int>? Ids)
		{
			using var _context = new WarehousesManagerContext();

			var query = await _context.Materials.AsNoTracking().Include(x => x.Category).Where(x => x.IsActive == true)
				.Where(x => Ids == null || !Ids.Any() || Ids.Contains(x.MaterialId))
				.Select(x => new MaterialResponeMessage
				{
					CategoryName = x.Category.CategoryName,
					MaterialId = x.MaterialId,
					MaterialName = x.MaterialName,
					Unit = x.Unit,

				}).ToListAsync();
			foreach (var item in query)
			{
				var totalInWarehouse = await _context.Stocks.Where(s => s.MaterialId == item.MaterialId).SumAsync(s => s.Quantity);
				item.Quantity = (int)totalInWarehouse;
			}
			return query;
		}
		public async Task<List<int>> GetMaterialInReceipt(int ReceiptId)
		{
			using var _context = new WarehousesManagerContext();
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
			using var _context = new WarehousesManagerContext();
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
		public async Task<bool> CreatedMaterialsAsync(CreatedMaterialRequestModel request)
		{
			try
			{
				using var _context = new WarehousesManagerContext();

				var dataNew = new Material()
				{
					CategoryId = request.CategoryId,
					Description = request.Description,
					MaterialCode = $"MT{DateTime.Now:yyyyMMddHHmmss}",
					MaterialName = request.Name,
					Quantity = request.Quantity,
					Unit = (decimal)request.Units,
				};
				await _context.AddAsync(dataNew);
				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public async Task<List<MaterialCategoryRespone>> MaterialCategoryAsync()
		{
			var resp = new List<MaterialCategoryRespone>();
			using var _context = new WarehousesManagerContext();
			try
			{
				var category = await _context.MaterialCategories.AsNoTracking().Select(x => new MaterialCategoryRespone
				{
					Id = x.CategoryId,
					Name = x.CategoryName,

				}).ToListAsync();

				resp = category.ToList();

				return resp;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public async Task<bool> UpdateMaterialsAsync(UpdateMaterialRequestModel request)
		{
			try
			{
				using var _context = new WarehousesManagerContext();

				var material = await _context.Materials
					.FirstOrDefaultAsync(x => x.MaterialId == request.Id);

				if (material == null)
					return false;

				// Update các field
				material.CategoryId = request.CategoryId;
				material.Description = request.Description;
				material.MaterialName = request.Name;
				material.Quantity = request.Quantity;
				material.Unit = (decimal)request.Units;

				await _context.SaveChangesAsync();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public async Task<bool> DeleteMaterialsAsync(List<int>? ids)
		{
			if (ids == null || !ids.Any())
				return false;

			using var _context = new WarehousesManagerContext();

			try
			{
				foreach (var item in ids)
				{
					var material = await _context.Materials
						.FirstOrDefaultAsync(x => x.MaterialId == item);

					if (material == null)
						continue; // Bỏ qua nếu không tìm thấy, tiếp tục xóa các item khác

					// Xóa tất cả các records liên quan (không chỉ record đầu tiên)
					var inventoryIssueDetails = await _context.InventoryIssueDetails
						.Where(x => x.MaterialId == item).ToListAsync();
					var inventoryReceiptDetails = await _context.InventoryReceiptDetails
						.Where(x => x.MaterialId == item).ToListAsync();
					var stockLogs = await _context.StockLogs
						.Where(x => x.MaterialId == item).ToListAsync();
					var stocks = await _context.Stocks
						.Where(x => x.MaterialId == item).ToListAsync();

					// Xóa các records liên quan
					if (stocks.Any())
						_context.Stocks.RemoveRange(stocks);
					if (stockLogs.Any())
						_context.StockLogs.RemoveRange(stockLogs);
					if (inventoryReceiptDetails.Any())
						_context.InventoryReceiptDetails.RemoveRange(inventoryReceiptDetails);
					if (inventoryIssueDetails.Any())
						_context.InventoryIssueDetails.RemoveRange(inventoryIssueDetails);

					// Xóa material (hard delete)
					_context.Materials.Remove(material);
				}
				await _context.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		public async Task<MaterialByIdResponeMessage> MaterialById(int materialId)
		{
			try
			{
				using var _context = new WarehousesManagerContext();
				var material = await _context.Materials.AsNoTracking().Include(x => x.Category)
						.FirstOrDefaultAsync(x => x.MaterialId == materialId);
				var resp = new MaterialByIdResponeMessage
				{
					CategoryName = material.Category.CategoryName ?? null,
					Description = material.Description,
					MaterialId = materialId,
					MaterialName = material.MaterialName,
					Quantity = material.Quantity,
					CategoryId = material.CategoryId,
					Unit = material.Unit
				};
				return resp;
			}
			catch { return null; }
		}
		public async Task<MaterialCategoryRespone> CategoryByIdRespone(int id)
		{
			using var _context = new WarehousesManagerContext();
			var material = await _context.MaterialCategories.AsNoTracking()
					.FirstOrDefaultAsync(x => x.CategoryId == id);
			if (material == null) return null;
			var resp = new MaterialCategoryRespone
			{
				Id = id,
				Name = material.CategoryName ?? null,
			};
			return resp;
		}
	}
}
