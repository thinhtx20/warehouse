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
				}
				var body = new ReceiptRequestModels()
				{
					CreatedBy = _currentUser.UserId,
					Desciptions = txtDescription.SelectedText,
					WarehouseId = int.Parse(cbWarehouse.SelectedValue.ToString()),
					Items = new List<ReceiptItem>()
				};
				foreach (DataGridViewRow row in dgvReceipts.Rows)
				{
					if (row.IsNewRow) continue;

					var selected = row.Cells["cbDgvReceiptForm"].Value;
					if (selected != null && (bool)selected == true)
					{
						body.Items.Add(new ReceiptItem()
						{
							MaterialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value),
							Quantity = Convert.ToInt32(row.Cells["Quantity"].Value),
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
			int selectedId = Convert.ToInt32(dgvListReceipt.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn1"].Value);
			if (e.RowIndex < 0)
			{
				LoadDataGridView();
				lstIds.Clear();
				return;
			}
			_warehouses.Clear();
			_warehouses = await _inventoryService.LoadMaterials(selectedId);
			// Combobox Warehouse
			cbWarehouse.DataSource = _warehouses;
			cbWarehouse.DisplayMember = "WarehouseName";
			cbWarehouse.ValueMember = "WarehouseId";
			cbWarehouse.SelectedIndex = 0; // chọn mặc định là item click	
			var idClick = Convert.ToInt32(dgvListReceipt.Rows[e.RowIndex].Cells["receiptIDDataGridViewTextBoxColumn"].Value);
			var lstIdMaterial = await _materialServices.GetMaterialInReceipt(idClick);
			lstIds.AddRange(lstIdMaterial);
			_materialData = await _materialServices.GetMaterialsAsync(lstIds);
			// update gridview 
			dgvReceipts.DataSource = null;
			dgvReceipts.DataSource = _materialData;
			// Tích tất cả checkbox nếu muốn mặc định
			foreach (DataGridViewRow row in dgvReceipts.Rows)
			{
				row.Cells["cbDgvReceiptForm"].Value = true;
			}
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			if (lstIds.Count > 1)
			{
				MessageBox.Show("Chỉ có thể chỉnh sửa 1 phiếu", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

		}
	}
}
