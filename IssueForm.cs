using Inventory_manager.CustomForm;
using Inventory_manager.dto.Request;
using Inventory_manager.dto.Response;
using Inventory_manager.Models;
using Inventory_manager.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_manager
{
	public partial class IssueForm : Form
	{
		private User _currentUser;
		private readonly InventoryService _issueService;
		private List<WarehouseMaterialRespone> _warehouses;
		private readonly MaterialServices _materialServices;
		private List<MaterialResponeMessage> _materialData = new List<MaterialResponeMessage>();
		private List<ListIssueResponseMessage> _issueData = new List<ListIssueResponseMessage>();
		private List<int> lstIds = new List<int>();
		private List<int> lstIdMaterial = new List<int>();
		private bool _isLoadingIssue = false; // Flag để tránh conflict khi load issue

		public IssueForm(User user)
		{
			_currentUser = user;
			_issueService = new InventoryService();
			_materialServices = new MaterialServices();
			InitializeComponent();
			this.Load += IssueForm_Load;
		}

		private async void IssueForm_Load(object sender, EventArgs e)
		{
			try
			{
				txtDescription.Clear();
				dtCreatedAt.Value = DateTime.Now; // Set ngày mặc định là ngày hiện tại
				await LoadMockData();
				LoadDataCombobox();
				LoadDataGridView();
				// Đăng ký event handler cho combobox warehouse
				cbWarehouse.SelectedIndexChanged += cbWarehouse_SelectedIndexChanged;
				// Đăng ký event handler cho validation số lượng
				dgvIssues.CellEndEdit += dgvIssues_CellEndEdit;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		private async Task LoadMockData()
		{
			try
			{
				_warehouses = await _issueService.LoadMaterials(null);
				// Mặc định không chọn kho thì danh sách vật liệu rỗng
				_materialData = new List<MaterialResponeMessage>();
				_issueData = await _issueService.GetIssueAsync();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadDataCombobox()
		{
			try
			{
				if (_warehouses == null) return;

				cbWarehouse.DataSource = _warehouses;
				cbWarehouse.DisplayMember = "WarehouseName";
				cbWarehouse.ValueMember = "WarehouseId";
				cbWarehouse.SelectedIndex = -1;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void LoadDataGridView()
		{
			dgvIssues.DataSource = null;
			dgvIssues.DataSource = _materialData;

			dgvListIssue.DataSource = null;
			dgvListIssue.DataSource = _issueData;
			
			// Format cột CreatedAt để hiển thị theo định dạng 24h
			if (dgvListIssue.Columns["CreatedAt"] != null)
			{
				dgvListIssue.Columns["CreatedAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
			}
		}

		private async void dgvListIssue_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			// Chống click header
			if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

			// Chỉ xử lý khi click vào checkbox (cột cbIssue)
			var column = dgvListIssue.Columns[e.ColumnIndex];
			if (column == null || column.Name != "cbIssue") return;

			var row = dgvListIssue.Rows[e.RowIndex];
			if (row.IsNewRow) return;

			// Lấy checkbox cell
			var checkboxCell = row.Cells["cbIssue"] as DataGridViewCheckBoxCell;
			if (checkboxCell == null) return;

			// Lấy selectedId từ DataGridView - sử dụng cột IssueIDDataGridViewTextBoxColumn
			var valueObj = row.Cells["IssueIDDataGridViewTextBoxColumn"].Value;
			if (valueObj == null) return;

			if (!int.TryParse(valueObj.ToString(), out int selectedId)) return;

			// Toggle checkbox value
			bool currentValue = (bool?)(checkboxCell.Value) ?? false;
			checkboxCell.Value = !currentValue;
			
			// Commit edit để đảm bảo giá trị checkbox được cập nhật
			dgvListIssue.CommitEdit(DataGridViewDataErrorContexts.Commit);

			// Đọc lại giá trị sau khi toggle
			bool isChecked = (bool?)(checkboxCell.Value) ?? false;

			// Nếu uncheck thì clear dữ liệu
			if (!isChecked)
			{
				lstIds.Remove(selectedId);
				// Clear form
				txtDescription.Clear();
				dtCreatedAt.Value = DateTime.Now;
				cbWarehouse.SelectedIndex = -1;
				_materialData.Clear();
				dgvIssues.DataSource = null;
				dgvIssues.DataSource = _materialData;
				return;
			}

			// Nếu đã có issue khác được chọn, uncheck nó
			if (lstIds.Any() && lstIds.Count == 1 && lstIds[0] != selectedId)
			{
				// Uncheck issue cũ
				foreach (DataGridViewRow r in dgvListIssue.Rows)
				{
					if (r.IsNewRow) continue;
					var oldIdCell = r.Cells["IssueIDDataGridViewTextBoxColumn"];
					if (oldIdCell != null && oldIdCell.Value != null)
					{
						if (int.TryParse(oldIdCell.Value.ToString(), out int oldId) && oldId == lstIds[0])
						{
							var oldCheckbox = r.Cells["cbIssue"] as DataGridViewCheckBoxCell;
							if (oldCheckbox != null)
							{
								oldCheckbox.Value = false;
							}
						}
					}
				}
				lstIds.Clear();
			}

			// Lấy warehouse ID từ issue
			var issue = _issueData.FirstOrDefault(x => x.IssueID == selectedId);
			if (issue == null || !issue.WarehouseID.HasValue) return;

			int warehouseId = issue.WarehouseID.Value;

			// Set flag để tránh trigger event khi đang load issue
			_isLoadingIssue = true;

			try
			{
				// Reset danh sách kho - load tất cả kho và chọn kho của issue
				_warehouses = await _issueService.LoadMaterials(null);
				cbWarehouse.DataSource = _warehouses;
				cbWarehouse.DisplayMember = "WarehouseName";
				cbWarehouse.ValueMember = "WarehouseId";
				
				// Chọn kho của issue
				for (int i = 0; i < cbWarehouse.Items.Count; i++)
				{
					var warehouse = cbWarehouse.Items[i] as WarehouseMaterialRespone;
					if (warehouse != null && warehouse.WarehouseId == warehouseId)
					{
						cbWarehouse.SelectedIndex = i;
						break;
					}
				}

				// Clear lstIds và thêm selectedId
				lstIds.Clear();
				lstIds.Add(selectedId);

				// Load chi tiết issue để lấy description, ngày và số lượng đã xuất
				using var _db = new WarehousesManagerContext();
				var issueEntity = await _db.InventoryIssues.AsNoTracking()
					.Include(x => x.InventoryIssueDetails)
					.FirstOrDefaultAsync(x => x.IssueId == selectedId);

				if (issueEntity != null)
				{
					// Load description vào textbox
					txtDescription.Text = issueEntity.Description ?? "";

					// Load ngày vào DateTimePicker
					if (issueEntity.CreatedAt.HasValue)
					{
						dtCreatedAt.Value = issueEntity.CreatedAt.Value;
					}
					else
					{
						dtCreatedAt.Value = DateTime.Now;
					}

					// Load TẤT CẢ vật liệu trong kho được chọn
					await LoadMaterialsByWarehouse(preserveIssueData: true);

					// Tính lại số lượng trong kho = số lượng hiện tại + số lượng đã xuất
					// để hiển thị số lượng ban đầu (trước khi xuất)
					// Sử dụng for loop thay vì foreach để tránh lỗi khi collection thay đổi
					for (int i = 0; i < dgvIssues.Rows.Count; i++)
					{
						var row1 = dgvIssues.Rows[i];
						if (row1 == null || row1.IsNewRow) continue;

						try
						{
							var materialIdCell = row1.Cells["materialIdDataGridViewTextBoxColumn"];
							if (materialIdCell == null || materialIdCell.Value == null || materialIdCell.Value == DBNull.Value) continue;

							if (!int.TryParse(materialIdCell.Value.ToString(), out int materialId)) continue;

							var detail = issueEntity.InventoryIssueDetails.FirstOrDefault(x => x.MaterialId == materialId);
							if (detail == null) continue;

							// Tính lại số lượng ban đầu = số lượng hiện tại + số lượng đã xuất
							var currentQuantityCell = row1.Cells["Quantity"];
							if (currentQuantityCell != null && currentQuantityCell.Value != null && currentQuantityCell.Value != DBNull.Value)
							{
								if (int.TryParse(currentQuantityCell.Value.ToString(), out int currentQuantity))
								{
									// Số lượng ban đầu = số lượng hiện tại + số lượng đã xuất
									if (detail.Quantity.HasValue)
									{
										int originalQuantity = currentQuantity + (int)detail.Quantity.Value;
										currentQuantityCell.Value = originalQuantity;
									}
								}
							}

							// Tick checkbox
							var cell = row1.Cells["select"] as DataGridViewCheckBoxCell;
							if (cell != null)
							{
								cell.Value = true;
							}
							// Set số lượng đã xuất vào cột Column1
							if (detail.Quantity.HasValue)
							{
                                row1.Cells["Column1"].Value = detail.Quantity.Value;
							}
						}
						catch (Exception ex)
						{
							// Log lỗi nhưng tiếp tục xử lý các row khác
							System.Diagnostics.Debug.WriteLine($"Error processing row {i}: {ex.Message}");
							continue;
						}
					}
				}
				dgvIssues.Refresh();
			}
			finally
			{
				_isLoadingIssue = false;
			}
		}

		private async void btnAdd_Click_1(object sender, EventArgs e)
		{
			try
			{
				if (cbWarehouse.SelectedValue == null || string.IsNullOrEmpty(cbWarehouse.SelectedValue.ToString()))
				{
					MessageBox.Show("Vui lòng chọn kho", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				var body = new IssueRequestModels()
				{
					CreatedBy = _currentUser.UserId,
					Desciptions = txtDescription.Text,
					WarehouseId = int.Parse(cbWarehouse.SelectedValue.ToString()),
					CreatedAt = dtCreatedAt.Value,
					Items = new List<IssueItemRequest>()
				};

				bool hasSelectedMaterial = false;
				foreach (DataGridViewRow row in dgvIssues.Rows)
				{
					if (row.IsNewRow) continue;

					var selected = row.Cells["select"].Value;
					if (selected != null && (bool)selected == true)
					{
						hasSelectedMaterial = true;
						
						// Lấy số lượng từ cột Column1 (số lượng nhập)
						var quantityCell = row.Cells["Column1"];
						if (quantityCell == null || quantityCell.Value == null || string.IsNullOrEmpty(quantityCell.Value.ToString()))
						{
							var materialName = row.Cells["materialNameDataGridViewTextBoxColumn"]?.Value?.ToString() ?? "vật liệu";
							MessageBox.Show($"Vui lòng nhập số lượng cho '{materialName}'", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							return;
						}

						if (!int.TryParse(quantityCell.Value.ToString(), out int quantity) || quantity <= 0)
						{
							var materialName = row.Cells["materialNameDataGridViewTextBoxColumn"]?.Value?.ToString() ?? "vật liệu";
							MessageBox.Show($"Số lượng của '{materialName}' phải là số nguyên lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							return;
						}

						// Kiểm tra lại số lượng không vượt quá tồn kho
						var quantityInStockCell = row.Cells["Quantity"];
						if (quantityInStockCell != null && quantityInStockCell.Value != null)
						{
							if (int.TryParse(quantityInStockCell.Value.ToString(), out int stockQuantity))
							{
								if (quantity > stockQuantity)
								{
									var materialName = row.Cells["materialNameDataGridViewTextBoxColumn"]?.Value?.ToString() ?? "vật liệu";
									MessageBox.Show($"Số lượng xuất của '{materialName}' ({quantity}) không được lớn hơn số lượng trong kho ({stockQuantity})", 
										"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
									return;
								}
							}
						}

						body.Items.Add(new IssueItemRequest()
						{
							MaterialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value),
							Quantity = quantity,
							UnitPrice = Convert.ToDecimal(row.Cells["unitDataGridViewTextBoxColumn"].Value)
						});
					}
				}

				if (!hasSelectedMaterial)
				{
					MessageBox.Show("Vui lòng chọn ít nhất một vật liệu", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				if (body.Items.Count == 0)
				{
					MessageBox.Show("Vui lòng chọn vật liệu và nhập số lượng", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				await _issueService.CreateIssue(body);
				MessageBox.Show("Thêm phiếu xuất hàng thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

				IssueForm_Load(this, EventArgs.Empty);
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private async void btnUpdate_Click(object sender, EventArgs e)
		{
			try
			{
				if (lstIds.Count > 1)
				{
					MessageBox.Show("Chỉ có thể chỉnh sửa 1 phiếu", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				if (!lstIds.Any())
				{
					MessageBox.Show("Vui lòng chọn phiếu cần sửa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				if (cbWarehouse.SelectedValue == null || string.IsNullOrEmpty(cbWarehouse.SelectedValue.ToString()))
				{
					MessageBox.Show("Vui lòng chọn kho", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				var body = new IssueUpdateRequestModels()
				{
					IssueId = lstIds.First(),
					WarehouseId = int.Parse(cbWarehouse.SelectedValue.ToString()),
					Desciptions = txtDescription.Text,
					Items = new List<IssueItemUpdateRequest>()
				};

				bool hasSelectedMaterial = false;
				foreach (DataGridViewRow row in dgvIssues.Rows)
				{
					if (row.IsNewRow) continue;

					var selected = row.Cells["select"].Value;
					if (selected != null && (bool)selected == true)
					{
						hasSelectedMaterial = true;

						// Lấy số lượng từ cột Column1 (số lượng nhập)
						var quantityCell = row.Cells["Column1"];
						if (quantityCell == null || quantityCell.Value == null || string.IsNullOrEmpty(quantityCell.Value.ToString()))
						{
							var materialName = row.Cells["materialNameDataGridViewTextBoxColumn"]?.Value?.ToString() ?? "vật liệu";
							MessageBox.Show($"Vui lòng nhập số lượng cho '{materialName}'", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							return;
						}

						if (!int.TryParse(quantityCell.Value.ToString(), out int quantity) || quantity <= 0)
						{
							var materialName = row.Cells["materialNameDataGridViewTextBoxColumn"]?.Value?.ToString() ?? "vật liệu";
							MessageBox.Show($"Số lượng của '{materialName}' phải là số nguyên lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							return;
						}

						// Kiểm tra lại số lượng không vượt quá tồn kho
						var quantityInStockCell = row.Cells["Quantity"];
						if (quantityInStockCell != null && quantityInStockCell.Value != null)
						{
							if (int.TryParse(quantityInStockCell.Value.ToString(), out int stockQuantity))
							{
								if (quantity > stockQuantity)
								{
									var materialName = row.Cells["materialNameDataGridViewTextBoxColumn"]?.Value?.ToString() ?? "vật liệu";
									MessageBox.Show($"Số lượng xuất của '{materialName}' ({quantity}) không được lớn hơn số lượng trong kho ({stockQuantity})", 
										"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
									return;
								}
							}
						}

						body.Items.Add(new IssueItemUpdateRequest()
						{
							MaterialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value),
							Quantity = quantity,
							UnitPrice = Convert.ToDecimal(row.Cells["unitDataGridViewTextBoxColumn"].Value)
						});
					}
				}

				if (!hasSelectedMaterial)
				{
					MessageBox.Show("Vui lòng chọn ít nhất một vật liệu", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				if (body.Items.Count == 0)
				{
					MessageBox.Show("Vui lòng chọn vật liệu và nhập số lượng", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				await _issueService.UpdateIssue(body);
				MessageBox.Show("Cập nhật phiếu xuất hàng thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

				IssueForm_Load(this, EventArgs.Empty);
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnDelete_Click_1(object sender, EventArgs e)
		{
			try
			{
				if (!lstIds.Any())
				{
					MessageBox.Show("Vui lòng chọn phiếu cần xóa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				_issueService.DeleteIssue(lstIds);
				MessageBox.Show("Xóa phiếu xuất hàng thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

				IssueForm_Load(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnBack_Click_1(object sender, EventArgs e)
		{
			var mainForm = new MainForm(_currentUser);
			mainForm.Show();
			lstIds.Clear();
			this.Close();
		}

		private void btnRefresh_Click_1(object sender, EventArgs e)
		{
			IssueForm_Load(this, EventArgs.Empty);
		}

		/// <summary>
		/// Xuất Excel cho phiếu xuất hàng đã chọn
		/// </summary>
		private async void btnExportExcel_Click(object sender, EventArgs e)
		{
			try
			{
				if (lstIds == null || !lstIds.Any())
				{
					MessageBox.Show("Vui lòng chọn phiếu xuất hàng cần xuất Excel", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				if (lstIds.Count > 1)
				{
					MessageBox.Show("Chỉ có thể xuất Excel 1 phiếu xuất hàng tại một thời điểm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
					return;
				}

				int issueId = lstIds.First();

				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
					saveFileDialog.FilterIndex = 1;
					saveFileDialog.FileName = $"PhieuXuatHang_{issueId}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
						await ExportIssueToExcel(issueId, saveFileDialog.FileName);
						MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Export chi tiết phiếu xuất hàng ra Excel
		/// </summary>
		private async Task ExportIssueToExcel(int issueId, string filePath)
		{
			using (var package = new ExcelPackage())
			{
				using var _db = new WarehousesManagerContext();
				
				// Load chi tiết phiếu xuất hàng
				var issue = await _db.InventoryIssues.AsNoTracking()
					.Include(x => x.CreatedByNavigation)
					.Include(x => x.Warehouse)
					.Include(x => x.InventoryIssueDetails)
						.ThenInclude(x => x.Material)
					.FirstOrDefaultAsync(x => x.IssueId == issueId);

				if (issue == null)
				{
					throw new Exception("Không tìm thấy phiếu xuất hàng");
				}

				var worksheet = package.Workbook.Worksheets.Add("Phiếu xuất hàng");

				// Header thông tin phiếu
				int currentRow = 1;
				worksheet.Cells[currentRow, 1].Value = "PHIẾU XUẤT HÀNG";
				worksheet.Cells[currentRow, 1, currentRow, 6].Merge = true;
				worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
				worksheet.Cells[currentRow, 1].Style.Font.Size = 16;
				worksheet.Cells[currentRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
				currentRow += 2;

				// Thông tin phiếu
				worksheet.Cells[currentRow, 1].Value = "Mã phiếu:";
				worksheet.Cells[currentRow, 2].Value = issue.IssueCode ?? "";
				worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
				currentRow++;

				worksheet.Cells[currentRow, 1].Value = "Kho:";
				worksheet.Cells[currentRow, 2].Value = issue.Warehouse?.WarehouseName ?? "";
				worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
				currentRow++;

				worksheet.Cells[currentRow, 1].Value = "Ngày xuất:";
				worksheet.Cells[currentRow, 2].Value = issue.CreatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "";
				worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
				currentRow++;

				worksheet.Cells[currentRow, 1].Value = "Người tạo:";
				worksheet.Cells[currentRow, 2].Value = issue.CreatedByNavigation?.FullName ?? "";
				worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
				currentRow++;

				if (!string.IsNullOrEmpty(issue.Description))
				{
					worksheet.Cells[currentRow, 1].Value = "Mô tả:";
					worksheet.Cells[currentRow, 2].Value = issue.Description;
					worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
					currentRow++;
				}

				currentRow += 1;

				// Header bảng chi tiết
				worksheet.Cells[currentRow, 1].Value = "STT";
				worksheet.Cells[currentRow, 2].Value = "Tên vật liệu";
				worksheet.Cells[currentRow, 3].Value = "Số lượng";
				worksheet.Cells[currentRow, 4].Value = "Đơn giá";
				worksheet.Cells[currentRow, 5].Value = "Thành tiền";

				using (var range = worksheet.Cells[currentRow, 1, currentRow, 5])
				{
					range.Style.Font.Bold = true;
					range.Style.Fill.PatternType = ExcelFillStyle.Solid;
					range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
					range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
					range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
					range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
				}

				currentRow++;

				// Dữ liệu chi tiết
				int stt = 1;
				decimal totalAmount = 0;

				foreach (var detail in issue.InventoryIssueDetails.OrderBy(x => x.MaterialId))
				{
					decimal quantity = detail.Quantity ?? 0;
					decimal unitPrice = detail.UnitPrice ?? 0;
					decimal amount = quantity * unitPrice;
					totalAmount += amount;

					worksheet.Cells[currentRow, 1].Value = stt;
					worksheet.Cells[currentRow, 2].Value = detail.Material?.MaterialName ?? "";
					worksheet.Cells[currentRow, 3].Value = quantity;
					worksheet.Cells[currentRow, 4].Value = unitPrice;
					worksheet.Cells[currentRow, 5].Value = amount;

					worksheet.Cells[currentRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells[currentRow, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
					worksheet.Cells[currentRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
					worksheet.Cells[currentRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

					// Format số
					worksheet.Cells[currentRow, 3].Style.Numberformat.Format = "#,##0";
					worksheet.Cells[currentRow, 4].Style.Numberformat.Format = "#,##0";
					worksheet.Cells[currentRow, 5].Style.Numberformat.Format = "#,##0";

					// Border
					using (var dataRange = worksheet.Cells[currentRow, 1, currentRow, 5])
					{
						dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
						dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
						dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
						dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
					}

					currentRow++;
					stt++;
				}

				// Tổng tiền
				currentRow++;
				worksheet.Cells[currentRow, 4].Value = "TỔNG TIỀN:";
				worksheet.Cells[currentRow, 4].Style.Font.Bold = true;
				worksheet.Cells[currentRow, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
				worksheet.Cells[currentRow, 5].Value = totalAmount;
				worksheet.Cells[currentRow, 5].Style.Font.Bold = true;
				worksheet.Cells[currentRow, 5].Style.Numberformat.Format = "#,##0";
				worksheet.Cells[currentRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

				// Border cho tổng tiền
				using (var totalRange = worksheet.Cells[currentRow, 4, currentRow, 5])
				{
					totalRange.Style.Border.Top.Style = ExcelBorderStyle.Thick;
					totalRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					totalRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
					totalRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
				}

				// Auto fit columns
				worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

				// Set độ rộng cột
				worksheet.Column(1).Width = 8;
				worksheet.Column(2).Width = 30;
				worksheet.Column(3).Width = 15;
				worksheet.Column(4).Width = 15;
				worksheet.Column(5).Width = 15;

				worksheet.Row(1).Height = 30;

				package.SaveAs(new FileInfo(filePath));
			}
		}

		/// <summary>
		/// Event handler khi chọn kho - load danh sách vật liệu trong kho đó
		/// </summary>
		private async void cbWarehouse_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				// Chỉ load khi không đang trong quá trình click vào issue cũ
				if (_isLoadingIssue) return;

				await LoadMaterialsByWarehouse();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Load tất cả vật liệu trong kho được chọn
		/// </summary>
		private async Task LoadMaterialsByWarehouse(bool preserveIssueData = false)
		{
			if (cbWarehouse.SelectedValue == null || string.IsNullOrEmpty(cbWarehouse.SelectedValue.ToString()))
			{
				// Không chọn kho thì hiển thị mảng rỗng
				_materialData.Clear();
				dgvIssues.DataSource = null;
				dgvIssues.DataSource = _materialData;
				return;
			}

			int warehouseId = int.Parse(cbWarehouse.SelectedValue.ToString());
			// Load TẤT CẢ vật liệu trong kho được chọn
			_materialData = await _materialServices.GetMaterialsByWarehouseAsync(warehouseId);
			
			dgvIssues.DataSource = null;
			dgvIssues.DataSource = _materialData;
			
			if (!preserveIssueData)
			{
				// Clear các giá trị số lượng nhập và uncheck checkbox
				foreach (DataGridViewRow row in dgvIssues.Rows)
				{
					if (row.IsNewRow) continue;
					row.Cells["Column1"].Value = null;
					var cell = row.Cells["select"] as DataGridViewCheckBoxCell;
					if (cell != null)
					{
						cell.Value = false;
					}
				}
			}
			dgvIssues.Refresh();
		}

		/// <summary>
		/// Validation số lượng nhập không được lớn hơn số lượng trong kho
		/// </summary>
		private void dgvIssues_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				// Chỉ validate cột số lượng (Column1)
				if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
				
				var column = dgvIssues.Columns[e.ColumnIndex];
				if (column == null || column.Name != "Column1") return;

				var row = dgvIssues.Rows[e.RowIndex];
				if (row.IsNewRow) return;

				// Kiểm tra đã chọn kho chưa
				if (cbWarehouse.SelectedValue == null || string.IsNullOrEmpty(cbWarehouse.SelectedValue.ToString()))
				{
					row.Cells["Column1"].Value = null;
					MessageBox.Show("Vui lòng chọn kho trước khi nhập số lượng", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				var quantityCell = row.Cells["Column1"];
				var quantityInStockCell = row.Cells["Quantity"];
				var materialIdCell = row.Cells["materialIdDataGridViewTextBoxColumn"];

				if (quantityCell == null || quantityInStockCell == null || materialIdCell == null) return;

				var quantityValue = quantityCell.Value;
				if (quantityValue == null || string.IsNullOrEmpty(quantityValue.ToString()))
				{
					return; // Cho phép để trống
				}

				if (!int.TryParse(quantityValue.ToString(), out int inputQuantity))
				{
					quantityCell.Value = null;
					MessageBox.Show("Số lượng phải là số nguyên", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				if (inputQuantity <= 0)
				{
					quantityCell.Value = null;
					MessageBox.Show("Số lượng phải lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				// Lấy số lượng trong kho
				var quantityInStock = quantityInStockCell.Value;
				if (quantityInStock == null || !int.TryParse(quantityInStock.ToString(), out int stockQuantity))
				{
					quantityCell.Value = null;
					MessageBox.Show("Không thể lấy số lượng trong kho", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// Validation: số lượng nhập không được lớn hơn số lượng trong kho
				if (inputQuantity > stockQuantity)
				{
					var materialName = row.Cells["materialNameDataGridViewTextBoxColumn"]?.Value?.ToString() ?? "vật liệu";
					quantityCell.Value = null;
					MessageBox.Show($"Số lượng xuất của '{materialName}' không được lớn hơn số lượng trong kho ({stockQuantity})", 
						"Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
