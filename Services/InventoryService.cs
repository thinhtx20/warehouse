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
				// 1. Lấy phiếu nhập
				var receipt = await _db.InventoryReceipts
					.Include(x => x.InventoryReceiptDetails)
					.FirstOrDefaultAsync(x => x.ReceiptId == request.RececiptId);

				if (receipt == null) return false;

				// 2. Hoàn tồn kho theo chi tiết cũ
				foreach (var old in receipt.InventoryReceiptDetails)
				{
					var stock = await _db.Stocks
						.FirstOrDefaultAsync(s => s.WarehouseId == receipt.WarehouseId
											   && s.MaterialId == old.MaterialId);

					if (stock != null)
					{
						stock.Quantity -= old.Quantity;
						stock.LastUpdated = DateTime.Now;
					}

					// log hoàn kho
					await _db.StockLogs.AddAsync(new StockLog
					{
						MaterialId = old.MaterialId,
						WarehouseId = receipt.WarehouseId,
						RefType = 1,                       // Phiếu nhập
						RefId = receipt.ReceiptId,
						QuantityChange = -old.Quantity,
						FinalQuantity = stock?.Quantity ?? 0,
						CreatedBy = receipt.CreatedBy,
						CreatedAt = DateTime.Now
					});
				}

				// 3. Xóa chi tiết cũ
				_db.InventoryReceiptDetails.RemoveRange(receipt.InventoryReceiptDetails);

				// 4. Thêm chi tiết mới + cập nhật tồn kho mới
				var newDetails = new List<InventoryReceiptDetail>();

				foreach (var item in request.Items)
				{
					// tạo chi tiết mới
					var newDetail = new InventoryReceiptDetail
					{
						ReceiptId = receipt.ReceiptId,
						MaterialId = item.MaterialId,
						Quantity = item.Quantity,
						UnitPrice = item.UnitPrice
					};

					newDetails.Add(newDetail);

					// cập nhật tồn kho
					var stock = await _db.Stocks
						.FirstOrDefaultAsync(s => s.WarehouseId == request.WarehouseId
											   && s.MaterialId == item.MaterialId);

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

					// log nhập kho mới
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

				// 5. Gán lại chi tiết
				receipt.InventoryReceiptDetails = newDetails;

				// 6. Lưu DB
				await _db.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		public async Task<bool> DeleteReceipt(List<int>? Ids)
		{
			if (Ids == null || !Ids.Any()) return false;

			try
			{
				foreach (var id in Ids)
				{
					var receipt = await _db.InventoryReceipts
						.Include(x => x.InventoryReceiptDetails)
						.FirstOrDefaultAsync(x => x.ReceiptId == id);

					if (receipt == null)
						continue;

					// 1. Trừ tồn kho theo từng chi tiết
					foreach (var item in receipt.InventoryReceiptDetails)
					{
						var stock = await _db.Stocks
							.FirstOrDefaultAsync(s =>
								s.WarehouseId == receipt.WarehouseId &&
								s.MaterialId == item.MaterialId);

						if (stock != null)
						{
							stock.Quantity -= item.Quantity;
							stock.LastUpdated = DateTime.Now;
						}

						// 2. Ghi log trừ kho
						await _db.StockLogs.AddAsync(new StockLog
						{
							MaterialId = item.MaterialId,
							WarehouseId = receipt.WarehouseId,
							RefType = 1,                // Phiếu nhập
							RefId = receipt.ReceiptId,
							QuantityChange = -item.Quantity,
							FinalQuantity = stock?.Quantity ?? 0,
							CreatedBy = receipt.CreatedBy,
							CreatedAt = DateTime.Now
						});
					}

					// 3. Xóa chi tiết phiếu
					_db.InventoryReceiptDetails.RemoveRange(receipt.InventoryReceiptDetails);

					// 4. Xóa phiếu
					_db.InventoryReceipts.Remove(receipt);
				}

				// 5. Lưu toàn bộ thay đổi
				await _db.SaveChangesAsync();

				return true;
			}
			catch (Exception ex)
			{
				throw;
			}
		}


	}
}
