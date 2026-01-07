using Inventory_manager.dto.Request;
using Inventory_manager.dto.Response;
using Inventory_manager.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory_manager.Services
{
	public class InventoryService
	{
		public InventoryService()
		{
			// Không cần DbContext ở constructor nữa
		}

		//  NHẬP 
		public async Task CreateReceipt(ReceiptRequestModels request)
		{
			if (request == null) return;

			using var _db = new WarehousesManagerContext();

			var receipt = new InventoryReceipt
			{
				ReceiptCode = $"PN{DateTime.Now:yyyyMMddHHmmss}",
				WarehouseId = request.WarehouseId,
				CreatedBy = request.CreatedBy,
				CreatedAt = request.CreatedAt ?? DateTime.Now,
				Description = request.Desciptions
			};

			foreach (var item in request.Items)
			{
				receipt.InventoryReceiptDetails.Add(new InventoryReceiptDetail
				{
					MaterialId = item.MaterialId,
					Quantity = item.Quantity,
					UnitPrice = item.UnitPrice
				});

				var stock = await _db.Stocks
					.FirstOrDefaultAsync(s => s.WarehouseId == request.WarehouseId && s.MaterialId == item.MaterialId);

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
			}

			await _db.InventoryReceipts.AddAsync(receipt);
			await _db.SaveChangesAsync();
		}

		public async Task<List<WarehouseMaterialRespone>> LoadMaterials(int? id)
		{
			using var _db = new WarehousesManagerContext();

			return await _db.Warehouses.AsNoTracking()
				.Where(x => !id.HasValue || x.WarehouseId == id.Value)
				.Select(x => new WarehouseMaterialRespone
				{
					WarehouseName = x.WarehouseName,
					WarehouseId = x.WarehouseId
				}).ToListAsync();
		}

		public async Task<List<ListReceiptResponeMessage>> GetReceiptAsync()
		{
			using var _db = new WarehousesManagerContext();

			var receipts = await _db.InventoryReceipts.AsNoTracking()
				.Include(x => x.CreatedByNavigation)
				.Include(x => x.Warehouse)
				.Select(x => new ListReceiptResponeMessage
				{
					CreatedBy = x.CreatedByNavigation.FullName ?? "",
					ReceiptID = x.ReceiptId,
					WarehouseDescription = x.Description,
					WarehouseID = x.WarehouseId,
					WarehouseName = x.Warehouse.WarehouseName ?? "",
					CreatedAt = x.CreatedAt
				}).ToListAsync();

			foreach (var item in receipts)
			{
				item.TotalMaterial = await _db.InventoryReceiptDetails
					.Where(x => x.ReceiptId == item.ReceiptID)
					.SumAsync(x => x.Quantity);
			}

			return receipts;
		}

		public async Task<InventoryReceipt> GetReceiptById(int receiptId)
		{
			using var _db = new WarehousesManagerContext();

			var receipt = await _db.InventoryReceipts
				.Include(x => x.InventoryReceiptDetails)
				.FirstOrDefaultAsync(x => x.ReceiptId == receiptId);

			return receipt;
		}

		public async Task<bool> UpdateReceipt(ReceiptUpdateRequestModels request)
		{
			if (request == null) return false;

			using var _db = new WarehousesManagerContext();

			var receipt = await _db.InventoryReceipts
				.Include(x => x.InventoryReceiptDetails)
				.FirstOrDefaultAsync(x => x.ReceiptId == request.RececiptId);

			if (receipt == null) return false;

			// Hoàn tồn kho cũ
			foreach (var old in receipt.InventoryReceiptDetails)
			{
				var stock = await _db.Stocks
					.FirstOrDefaultAsync(s => s.WarehouseId == receipt.WarehouseId && s.MaterialId == old.MaterialId);

				if (stock != null)
				{
					stock.Quantity -= old.Quantity;
					stock.LastUpdated = DateTime.Now;
				}

				await _db.StockLogs.AddAsync(new StockLog
				{
					MaterialId = old.MaterialId,
					WarehouseId = receipt.WarehouseId,
					RefType = 1,
					RefId = receipt.ReceiptId,
					QuantityChange = -old.Quantity,
					FinalQuantity = stock?.Quantity ?? 0,
					CreatedBy = receipt.CreatedBy,
					CreatedAt = DateTime.Now
				});
			}

			_db.InventoryReceiptDetails.RemoveRange(receipt.InventoryReceiptDetails);

			// Thêm chi tiết mới
			var newDetails = new List<InventoryReceiptDetail>();
			foreach (var item in request.Items)
			{
				var newDetail = new InventoryReceiptDetail
				{
					ReceiptId = receipt.ReceiptId,
					MaterialId = item.MaterialId,
					Quantity = item.Quantity,
					UnitPrice = item.UnitPrice
				};

				newDetails.Add(newDetail);

				var stock = await _db.Stocks
					.FirstOrDefaultAsync(s => s.WarehouseId == request.WarehouseId && s.MaterialId == item.MaterialId);

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

				await _db.StockLogs.AddAsync(new StockLog
				{
					MaterialId = item.MaterialId,
					WarehouseId = request.WarehouseId,
					RefType = 1,
					RefId = receipt.ReceiptId,
					QuantityChange = item.Quantity,
					FinalQuantity = stock.Quantity,
					CreatedBy = receipt.CreatedBy,
					CreatedAt = DateTime.Now
				});
			}

			receipt.InventoryReceiptDetails = newDetails;
			
			// Cập nhật mô tả và ngày tạo nếu có
			if (!string.IsNullOrEmpty(request.Desciptions))
			{
				receipt.Description = request.Desciptions;
			}
			if (request.CreatedAt.HasValue)
			{
				receipt.CreatedAt = request.CreatedAt.Value;
			}
			if (receipt.WarehouseId != request.WarehouseId)
			{
				receipt.WarehouseId = request.WarehouseId;
			}
			
			await _db.SaveChangesAsync();

			return true;
		}

		public async Task<bool> DeleteReceipt(List<int>? Ids)
		{
			if (Ids == null || !Ids.Any()) return false;

			using var _db = new WarehousesManagerContext();

			foreach (var id in Ids)
			{
				var receipt = await _db.InventoryReceipts
					.Include(x => x.InventoryReceiptDetails)
					.FirstOrDefaultAsync(x => x.ReceiptId == id);

				if (receipt == null) continue;

				foreach (var item in receipt.InventoryReceiptDetails)
				{
					var stock = await _db.Stocks
						.FirstOrDefaultAsync(s => s.WarehouseId == receipt.WarehouseId && s.MaterialId == item.MaterialId);

					if (stock != null)
					{
						stock.Quantity -= item.Quantity;
						stock.LastUpdated = DateTime.Now;
					}

					await _db.StockLogs.AddAsync(new StockLog
					{
						MaterialId = item.MaterialId,
						WarehouseId = receipt.WarehouseId,
						RefType = 1,
						RefId = receipt.ReceiptId,
						QuantityChange = -item.Quantity,
						FinalQuantity = stock?.Quantity ?? 0,
						CreatedBy = receipt.CreatedBy,
						CreatedAt = DateTime.Now
					});
				}

				_db.InventoryReceiptDetails.RemoveRange(receipt.InventoryReceiptDetails);
				_db.InventoryReceipts.Remove(receipt);
			}

			await _db.SaveChangesAsync();
			return true;
		}

		// XUẤT
		public async Task<bool> CreateIssue(IssueRequestModels request)
		{
			if (request == null) return false;

			using var _db = new WarehousesManagerContext();

			var issue = new InventoryIssue
			{
				IssueCode = $"PX{DateTime.Now:yyyyMMddHHmmss}",
				WarehouseId = request.WarehouseId,
				CreatedBy = request.CreatedBy,
				CreatedAt = request.CreatedAt ?? DateTime.Now,
				Description = request.Desciptions
			};

			foreach (var item in request.Items)
			{
				issue.InventoryIssueDetails.Add(new InventoryIssueDetail
				{
					MaterialId = item.MaterialId,
					Quantity = item.Quantity,
					UnitPrice = item.UnitPrice
				});

				var stock = await _db.Stocks
					.FirstOrDefaultAsync(s => s.WarehouseId == request.WarehouseId && s.MaterialId == item.MaterialId);

				if (stock == null || stock.Quantity < item.Quantity)
					throw new Exception("Tồn kho không đủ để xuất");

				stock.Quantity -= item.Quantity;
				stock.LastUpdated = DateTime.Now;

				await _db.StockLogs.AddAsync(new StockLog
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

			await _db.InventoryIssues.AddAsync(issue);
			await _db.SaveChangesAsync();

			return true;
		}

		public async Task<List<ListIssueResponseMessage>> GetIssueAsync()
		{
			using var _db = new WarehousesManagerContext();

			var issues = await _db.InventoryIssues.AsNoTracking()
				.Include(x => x.CreatedByNavigation)
				.Include(x => x.Warehouse)
				.Select(x => new ListIssueResponseMessage
				{
					CreatedBy = x.CreatedByNavigation.FullName,
					IssueID = x.IssueId,
					WarehouseDescription = x.Description,
					WarehouseID = x.WarehouseId,
					WarehouseName = x.Warehouse.WarehouseName,
					CreatedAt = x.CreatedAt
				}).ToListAsync();

			foreach (var item in issues)
			{
				item.TotalMaterial = await _db.InventoryIssueDetails
					.Where(x => x.IssueId == item.IssueID)
					.SumAsync(x => x.Quantity.Value);
			}

			return issues;
		}

		public async Task<bool> UpdateIssue(IssueUpdateRequestModels request)
		{
			if (request == null) return false;

			using var _db = new WarehousesManagerContext();

			var issue = await _db.InventoryIssues
				.Include(x => x.InventoryIssueDetails)
				.FirstOrDefaultAsync(x => x.IssueId == request.IssueId);

			if (issue == null) return false;

			// Hoàn kho cũ
			foreach (var old in issue.InventoryIssueDetails)
			{
				var stock = await _db.Stocks
					.FirstOrDefaultAsync(s => s.WarehouseId == issue.WarehouseId && s.MaterialId == old.MaterialId);

				if (stock != null)
				{
					stock.Quantity += old.Quantity;
					stock.LastUpdated = DateTime.Now;
				}

				await _db.StockLogs.AddAsync(new StockLog
				{
					MaterialId = old.MaterialId,
					WarehouseId = issue.WarehouseId,
					RefType = 2,
					RefId = issue.IssueId,
					QuantityChange = old.Quantity,
					FinalQuantity = stock?.Quantity ?? 0,
					CreatedBy = issue.CreatedBy,
					CreatedAt = DateTime.Now
				});
			}

			_db.InventoryIssueDetails.RemoveRange(issue.InventoryIssueDetails);

			var newDetails = new List<InventoryIssueDetail>();
			foreach (var item in request.Items)
			{
				newDetails.Add(new InventoryIssueDetail
				{
					IssueId = issue.IssueId,
					MaterialId = item.MaterialId,
					Quantity = item.Quantity,
					UnitPrice = item.UnitPrice
				});

				var stock = await _db.Stocks
					.FirstOrDefaultAsync(s => s.WarehouseId == issue.WarehouseId && s.MaterialId == item.MaterialId);

				if (stock == null || stock.Quantity < item.Quantity)
					throw new Exception("Tồn kho không đủ để xuất");

				stock.Quantity -= item.Quantity;
				stock.LastUpdated = DateTime.Now;

				await _db.StockLogs.AddAsync(new StockLog
				{
					MaterialId = item.MaterialId,
					WarehouseId = issue.WarehouseId,
					RefType = 2,
					RefId = issue.IssueId,
					QuantityChange = -item.Quantity,
					FinalQuantity = stock.Quantity,
					CreatedBy = issue.CreatedBy,
					CreatedAt = DateTime.Now
				});
			}

			issue.InventoryIssueDetails = newDetails;
			// Cập nhật description nếu có
			if (!string.IsNullOrEmpty(request.Desciptions))
			{
				issue.Description = request.Desciptions;
			}
			await _db.SaveChangesAsync();

			return true;
		}

		public async Task<bool> DeleteIssue(List<int>? Ids)
		{
			if (Ids == null || !Ids.Any()) return false;

			using var _db = new WarehousesManagerContext();

			foreach (var id in Ids)
			{
				var issue = await _db.InventoryIssues
					.Include(x => x.InventoryIssueDetails)
					.FirstOrDefaultAsync(x => x.IssueId == id);

				if (issue == null) continue;

				foreach (var item in issue.InventoryIssueDetails)
				{
					var stock = await _db.Stocks
						.FirstOrDefaultAsync(s => s.WarehouseId == issue.WarehouseId && s.MaterialId == item.MaterialId);

					if (stock != null)
					{
						stock.Quantity += item.Quantity;
						stock.LastUpdated = DateTime.Now;
					}

					await _db.StockLogs.AddAsync(new StockLog
					{
						MaterialId = item.MaterialId,
						WarehouseId = issue.WarehouseId,
						RefType = 2,
						RefId = issue.IssueId,
						QuantityChange = item.Quantity,
						FinalQuantity = stock?.Quantity ?? 0,
						CreatedBy = issue.CreatedBy,
						CreatedAt = DateTime.Now
					});
				}

				_db.InventoryIssueDetails.RemoveRange(issue.InventoryIssueDetails);
				_db.InventoryIssues.Remove(issue);
			}

			await _db.SaveChangesAsync();
			return true;
		}
	}
}
