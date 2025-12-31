using Inventory_manager.CustomForm;
using Inventory_manager.dto.Request;
using Inventory_manager.dto.Response;
using Inventory_manager.Models;
using Inventory_manager.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Inventory_manager
{
	public partial class ReceiptForm : Form
	{
		private User _currentUser;
		private readonly InventoryService _inventoryService = new InventoryService();
		private List<WarehouseMaterialRespone> _warehouses;
		private readonly MaterialServices _materialServices;
		private List<MaterialResponeMessage> _materialData = new List<MaterialResponeMessage>();
		private List<ListReceiptResponeMessage> _receiptData = new List<ListReceiptResponeMessage>();
		private List<int> lstIds = new List<int>();

		public ReceiptForm(User user)
		{

			_currentUser = user;
			_inventoryService = new InventoryService();
			_materialServices = new MaterialServices();
			InitializeComponent();
			this.Load += ReceiptForm_Load;
		}
		// hàm load 
		private async void ReceiptForm_Load(object sender, EventArgs e)
		{
			txtDescription.Clear();
			await LoadMockData();      // đợi dữ liệu load xong // sau đó mới gán cho ComboBox

			LoadDataCombobox();
			LoadDataGridView();
		}

		private async void LoadDataCombobox()
		{
			try
			{
				if (_warehouses == null) return;

				// Combobox Warehouse
				cbWarehouse.DataSource = _warehouses;
				cbWarehouse.DisplayMember = "WarehouseName";
				cbWarehouse.ValueMember = "WarehouseId";
				cbWarehouse.SelectedIndex = -1; // Không chọn mặc định
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void LoadDataGridView()
		{
			dgvReceipts.DataSource = null;
			dgvReceipts.DataSource = _materialData;
			///
			dgvListReceipt.DataSource = null;
			dgvListReceipt.DataSource = _receiptData;

		}
		private void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				if (cbWarehouse.SelectedValue is null || string.IsNullOrEmpty(cbWarehouse.SelectedValue.ToString()))
				{
					MessageBox.Show("Vui lòng chọn kho", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				var body = new ReceiptRequestModels()
				{
					CreatedBy = _currentUser.UserId,
					Desciptions = txtDescription.Text,
					WarehouseId = int.Parse(cbWarehouse.SelectedValue.ToString()),
					CreatedAt = dtCreatedAt.Value,
					Items = new List<ReceiptItem>()
				};
				foreach (DataGridViewRow row in dgvReceipts.Rows)
				{
					if (row.IsNewRow) continue;

					var selected = row.Cells["cbDgvReceiptForm"].Value;
					if (selected != null && (bool)selected == true)
					{
						var quantityReceiptValue = row.Cells["QuantityReceipt"].Value;
						if (quantityReceiptValue == null || string.IsNullOrEmpty(quantityReceiptValue.ToString()))
						{
							MessageBox.Show("Vui lòng nhập số lượng cho vật tư đã chọn", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							return;
						}
						body.Items.Add(new ReceiptItem()
						{
							MaterialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value),
							Quantity = Convert.ToInt32(quantityReceiptValue),
							UnitPrice = Convert.ToDecimal(row.Cells["unitDataGridViewTextBoxColumn"].Value)
						});
					}
				}
				_inventoryService.CreateReceipt(body);
				MessageBox.Show("Thêm phiếu thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
				// load form khi ta
				ReceiptForm_Load(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

		}
		//
		private async Task LoadMockData()
		{
			try
			{
				_warehouses = await _inventoryService.LoadMaterials(null);
				_materialData = await _materialServices.GetMaterialsAsync(null);
				_receiptData = await _inventoryService.GetReceiptAsync();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
		private void btnBack_Click(object sender, EventArgs e)
		{
			var mainForm = new MainForm(_currentUser);
			mainForm.Show();
			lstIds.Clear();
			this.Close();
		}
		private async void dgvListReceipt_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				LoadDataGridView();
				lstIds.Clear();
				return;
			}

			var idClick = Convert.ToInt32(dgvListReceipt.Rows[e.RowIndex].Cells["receiptIDDataGridViewTextBoxColumn"].Value);
			lstIds.Clear();
			lstIds.Add(idClick);

			// Lấy thông tin phiếu nhập
			var receipt = await _inventoryService.GetReceiptById(idClick);
			if (receipt == null) return;

			// Load mô tả phiếu
			txtDescription.Text = receipt.Description ?? "";

			// Load ngày tạo
			if (receipt.CreatedAt.HasValue)
			{
				dtCreatedAt.Value = receipt.CreatedAt.Value;
			}

			// Load warehouse
			if (receipt.WarehouseId.HasValue)
			{
				_warehouses.Clear();
				_warehouses = await _inventoryService.LoadMaterials(receipt.WarehouseId.Value);
				cbWarehouse.DataSource = _warehouses;
				cbWarehouse.DisplayMember = "WarehouseName";
				cbWarehouse.ValueMember = "WarehouseId";
				cbWarehouse.SelectedValue = receipt.WarehouseId.Value;
			}

			// Load danh sách vật tư trong phiếu
			var lstIdMaterial = await _materialServices.GetMaterialInReceipt(idClick);
			_materialData = await _materialServices.GetMaterialsAsync(lstIdMaterial);
			
			// Update gridview 
			dgvReceipts.DataSource = null;
			dgvReceipts.DataSource = _materialData;

			// Load số lượng từ receipt detail vào cột QuantityReceipt và tích checkbox
			foreach (DataGridViewRow row in dgvReceipts.Rows)
			{
				if (row.IsNewRow) continue;

				var materialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value);
				var receiptDetail = receipt.InventoryReceiptDetails.FirstOrDefault(x => x.MaterialId == materialId);
				
				if (receiptDetail != null)
				{
					// Set số lượng vào cột QuantityReceipt
					row.Cells["QuantityReceipt"].Value = receiptDetail.Quantity;
					// Tích checkbox
					row.Cells["cbDgvReceiptForm"].Value = true;
				}
			}
		}

		private async void btnUpdate_Click(object sender, EventArgs e)
		{
			try
			{
				if (!lstIds.Any())
				{
					MessageBox.Show("Vui lòng chọn phiếu cần sửa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				if (lstIds.Count > 1)
				{
					MessageBox.Show("Chỉ có thể chỉnh sửa 1 phiếu", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				if (cbWarehouse.SelectedValue is null || string.IsNullOrEmpty(cbWarehouse.SelectedValue.ToString()))
				{
					MessageBox.Show("Vui lòng chọn kho", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				var body = new ReceiptUpdateRequestModels()
				{
					RececiptId = lstIds.First(),
					WarehouseId = int.Parse(cbWarehouse.SelectedValue.ToString()),
					Desciptions = txtDescription.Text,
					CreatedAt = dtCreatedAt.Value,
					Items = new List<ReceiptItem>()
				};
				foreach (DataGridViewRow row in dgvReceipts.Rows)
				{
					if (row.IsNewRow) continue;

					var selected = row.Cells["cbDgvReceiptForm"].Value;
					if (selected != null && (bool)selected == true)
					{
						var quantityReceiptValue = row.Cells["QuantityReceipt"].Value;
						if (quantityReceiptValue == null || string.IsNullOrEmpty(quantityReceiptValue.ToString()))
						{
							MessageBox.Show("Vui lòng nhập số lượng cho vật tư đã chọn", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							return;
						}
						body.Items.Add(new ReceiptItem()
						{
							MaterialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value),
							Quantity = Convert.ToInt32(quantityReceiptValue),
							UnitPrice = Convert.ToDecimal(row.Cells["unitDataGridViewTextBoxColumn"].Value)
						});
					}
				}
				if (!body.Items.Any())
				{
					MessageBox.Show("Vui lòng chọn ít nhất một vật tư", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				var result = await _inventoryService.UpdateReceipt(body);
				if (result)
				{
					MessageBox.Show("Cập nhật phiếu thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
					// load form khi ta
					ReceiptForm_Load(this, EventArgs.Empty);
				}
				else
				{
					MessageBox.Show("Cập nhật phiếu thất bại", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			try
			{
				if (!lstIds.Any())
				{
					MessageBox.Show("Vui lòng chọn phiếu cần sửa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				_inventoryService.DeleteReceipt(lstIds);
				MessageBox.Show("Xóa phiếu thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
				// load form khi ta
				ReceiptForm_Load(this, EventArgs.Empty);
			}
			catch (Exception ex) 
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{

			ReceiptForm_Load(this, EventArgs.Empty);
		}
	}
}
