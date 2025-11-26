using Inventory_manager.CustomForm;
using Inventory_manager.dto.Request;
using Inventory_manager.dto.Response;
using Inventory_manager.Models;
using Inventory_manager.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
				await LoadMockData();
				LoadDataCombobox();
				LoadDataGridView();
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
				_materialData = await _materialServices.GetMaterialsAsync(null);
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
		}

		private async void dgvListIssue_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
			{
				LoadDataGridView();
				lstIds.Clear();
				return;
			}

			int selectedId = Convert.ToInt32(dgvListIssue.Rows[e.RowIndex].Cells["dataGridViewTextBoxColumn1"].Value);
			_warehouses.Clear();
			_warehouses = await _issueService.LoadMaterials(selectedId);

			cbWarehouse.DataSource = _warehouses;
			cbWarehouse.DisplayMember = "WarehouseName";
			cbWarehouse.ValueMember = "WarehouseId";
			cbWarehouse.SelectedIndex = 0;

			lstIds.Clear();
			lstIds.Add(selectedId);

			var lstIdMaterial = await _materialServices.GetMaterialInIssue(selectedId);
			_materialData = await _materialServices.GetMaterialsAsync(lstIdMaterial);

			dgvIssues.DataSource = null;
			dgvIssues.DataSource = _materialData;

			foreach (DataGridViewRow row in dgvIssues.Rows)
			{
				row.Cells["cbDgvIssueForm"].Value = true;
			}
		}

		private void btnAdd_Click_1(object sender, EventArgs e)
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
					Items = new List<IssueItemRequest>()
				};

				foreach (DataGridViewRow row in dgvIssues.Rows)
				{
					if (row.IsNewRow) continue;

					var selected = row.Cells["cbDgvIssueForm"].Value;
					if (selected != null && (bool)selected == true)
					{
						body.Items.Add(new IssueItemRequest()
						{
							MaterialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value),
							Quantity = Convert.ToInt32(row.Cells["Quantity"].Value),
							UnitPrice = Convert.ToDecimal(row.Cells["unitDataGridViewTextBoxColumn"].Value)
						});
					}
				}

				_issueService.CreateIssue(body);
				MessageBox.Show("Thêm phiếu xuất hàng thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

				IssueForm_Load(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnUpdate_Click(object sender, EventArgs e)
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

				var body = new IssueUpdateRequestModels()
				{
					IssueId = lstIds.First(),
					WarehouseId = cbWarehouse.SelectedIndex,
					Items = new List<IssueItemUpdateRequest>()
				};

				foreach (DataGridViewRow row in dgvIssues.Rows)
				{
					if (row.IsNewRow) continue;

					var selected = row.Cells["cbDgvIssueForm"].Value;
					if (selected != null && (bool)selected == true)
					{
						body.Items.Add(new IssueItemUpdateRequest()
						{
							MaterialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value),
							Quantity = Convert.ToInt32(row.Cells["Quantity"].Value),
							UnitPrice = Convert.ToDecimal(row.Cells["unitDataGridViewTextBoxColumn"].Value)
						});
					}
				}

				_issueService.UpdateIssue(body);
				MessageBox.Show("Cập nhật phiếu xuất hàng thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

				IssueForm_Load(this, EventArgs.Empty);
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
	}
}
