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
	public class InventoryService
	{
		private readonly WarehousesManagerContext _db = new WarehousesManagerContext();
		public InventoryService()
		{
		}
		// Nhập kho
		public async void CreateReceipt(ReceiptRequestModels request)
		{
			var receipt = new InventoryReceipt
			{
				ReceiptCode = $"PN{DateTime.Now:yyyyMMddHHmmss}",
				WarehouseId = request.WarehouseId,
				CreatedBy = request.CreatedBy,
				CreatedAt = DateTime.Now
			};

			foreach (var item in request.Items)
			{
				receipt.InventoryReceiptDetails.Add(new InventoryReceiptDetail
				{
					MaterialId = item.MaterialId,
					Quantity = item.Quantity,
					UnitPrice = item.UnitPrice
				});

				// Cập nhật tồn kho
				var stock = _db.Stocks.FirstOrDefault(s => s.WarehouseId == request.WarehouseId && s.MaterialId == item.MaterialId);
				if (stock == null)
				{
					stock = new Stock
					{
						WarehouseId = request.WarehouseId,
						MaterialId = item.MaterialId,
						Quantity = item.Quantity,
						LastUpdated = DateTime.Now
					};
					await _db.Stocks.AddAsync(stock);
				}
				else
				{
					stock.Quantity += item.Quantity;
					stock.LastUpdated = DateTime.Now;
				}

				// Lưu log
				await _db.StockLogs.AddAsync(new StockLog
				{
					MaterialId = item.MaterialId,
					WarehouseId = request.WarehouseId,
					RefType = 1,
					RefId = receipt.ReceiptId,
					QuantityChange = item.Quantity,
					FinalQuantity = stock.Quantity,
					CreatedBy = request.CreatedBy,
					CreatedAt = DateTime.Now
				});
				//var material = await _db.Materials.FirstOrDefaultAsync(x => x.MaterialId == item.MaterialId);
				//if (material != null)
				//{
				//	material.Quantity += item.Quantity;
				//}
			}

			await _db.InventoryReceipts.AddAsync(receipt);
			await _db.SaveChangesAsync();
		}

		// Xuất kho
		public bool CreateIssue(ReceiptRequestModels request)
		{
			var issue = new InventoryIssue
			{
				IssueCode = $"PX{DateTime.Now:yyyyMMddHHmmss}",
				WarehouseId = request.WarehouseId,
				CreatedBy = request.CreatedBy,
				CreatedAt = DateTime.Now
			};

			foreach (var item in request.Items)
			{
				var stock = _db.Stocks.FirstOrDefault(s => s.WarehouseId == request.WarehouseId && s.MaterialId == item.MaterialId);
				if (stock == null || stock.Quantity < item.Quantity)
					return false; // Không đủ tồn

				issue.InventoryIssueDetails.Add(new InventoryIssueDetail
				{
					MaterialId = item.MaterialId,
					Quantity = item.Quantity,
					UnitPrice = item.UnitPrice
				});

				stock.Quantity -= item.Quantity;
				stock.LastUpdated = DateTime.Now;

				_db.StockLogs.Add(new StockLog
				{
					MaterialId = item.MaterialId,
					WarehouseId = request.WarehouseId,
					RefType = 2,
					RefId = issue.IssueId,
					QuantityChange = -item.Quantity,
					FinalQuantity = stock.Quantity,
					CreatedBy = request.CreatedBy,
					CreatedAt = DateTime.Now
				});
			}

			_db.InventoryIssues.Add(issue);
			_db.SaveChanges();
			return true;
		}
		public async Task<List<WarehouseMaterialRespone>> LoadMaterials(int? id)
		{
			var materials = await _db.Warehouses.AsNoTracking().Where(x => !id.HasValue || x.WarehouseId == id.Value)
				.Select(x => new WarehouseMaterialRespone
				{
					WarehouseName = x.WarehouseName,
					WarehouseId = x.WarehouseId
				}).ToListAsync();
			return materials;
		}
		public async Task<List<ListReceiptResponeMessage>> GetReceiptAsync()
		{
			var result = new List<ListReceiptResponeMessage>();
			try
			{
				var query = await _db.InventoryReceipts.AsNoTracking().Include(x => x.CreatedByNavigation)
					.Select(x => new ListReceiptResponeMessage
					{
						CreatedBy = x.CreatedByNavigation.FullName ?? "",
						ReceiptID = x.ReceiptId,
						WarehouseDescription = x.Description,
						WarehouseID = x.WarehouseId,
						WarehouseName = x.Warehouse.WarehouseName ?? "",
						CreatedAt = x.CreatedAt,
					}).ToListAsync();
				foreach (var item in query)
				{
					var totalCount = await _db.InventoryReceiptDetails.Where(x => x.ReceiptId == item.ReceiptID).Select(x => new
					{
						x.ReceiptId,
						x.Quantity,
						x.MaterialId,
					}).ToListAsync();
					item.TotalMaterial = totalCount.Sum(x => x.Quantity);
				}
				result = query.ToList();
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception($"Message {ex.Message}");
			}
		}
		public async Task<bool> UpdateReceipt(ReceiptUpdateRequestModels request)
		{
			if (request == null) return false;
			try
			{

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
