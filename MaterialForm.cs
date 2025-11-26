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
	public partial class MaterialForm : Form
	{
		private readonly List<MaterialCategoryRespone> _dataCombobox = new List<MaterialCategoryRespone>();
		private List<MaterialResponeMessage> _materialData = new List<MaterialResponeMessage>();
		private readonly MaterialServices _materialServices;
		private List<int> lstIds = new List<int>();
		private User _currentUser;
		public MaterialForm(User user)
		{
			_currentUser = user;
			_materialServices = new MaterialServices();
			InitializeComponent();
			this.Load += FormLoad;
		}
		public async void FormLoad(object sender, EventArgs e)
		{
			await LoadData();
			LoadCombobox();
			LoadGirdView();
		}
		public async Task LoadData()
		{
			var data = await _materialServices.MaterialCategoryAsync();

			_dataCombobox.Clear();
			_dataCombobox.AddRange(data);
			_materialData.Clear();
			_materialData = await _materialServices.GetMaterialsAsync(null);
		}
		public void LoadCombobox()
		{
			cbbCategory.DataSource = _dataCombobox;
			cbbCategory.DisplayMember = "Name";
			cbbCategory.SelectedIndex = -1;
			cbbCategory.ValueMember = "Id";
		}
		public void LoadGirdView()
		{
			dgvMaterials.DataSource = null;
			dgvMaterials.DataSource = _materialData;
			///
		}
		private void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				var categoryId = cbbCategory.SelectedValue;
				if (categoryId is null || string.IsNullOrEmpty(categoryId.ToString()))
				{
					MessageBox.Show("Vui lòng chọn danh mục", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				if (string.IsNullOrEmpty(txtMaterialName.Text))
				{
					MessageBox.Show("Vui lòng chọn nhập tên danh mục", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				if (nbQuantity.Value <= 0)
				{
					MessageBox.Show("Số lượng lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				if (nbUnits.Value <= 0)
				{
					MessageBox.Show("Giá nhập lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				var request = new CreatedMaterialRequestModel()
				{
					Name = txtMaterialName.Text,
					CategoryId = Convert.ToInt32(categoryId.ToString()),
					Description = txtDescription.Text,
					Quantity = Convert.ToInt32(nbQuantity.Value),
					Units = Convert.ToInt32(nbUnits.Value),
				};
				_materialServices.CreatedMaterialsAsync(request);
				// load form khi ta
				FormLoad(this, EventArgs.Empty);
				return;
			}
			catch (Exception ex)
			{
				return;
			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			txtDescription.Clear();
			txtMaterialName.Clear();
			cbbCategory.SelectedIndex = -1;
			nbQuantity.Value = 0;
			nbUnits.Value = 0;

		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (lstIds.Any())
			{
				_materialServices.DeleteMaterialsAsync(lstIds);
				// load form khi ta
				FormLoad(this, EventArgs.Empty);
			}
		}

		private void dgvMaterials_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			var idClick = Convert.ToInt32(dgvMaterials.Rows[e.RowIndex].Cells["materialCick"].Value);
			lstIds.Add(idClick);
			var materialId = _materialServices.MaterialById(idClick);
			if (materialId != null)
			{
				txtDescription.Text = materialId.Result.Description;
				txtMaterialName.Text = materialId.Result.MaterialName;
				nbQuantity.Value = materialId.Result.Quantity;
				nbUnits.Value = materialId.Result.Unit;
				var category = _materialServices.CategoryByIdRespone(materialId.Result.CategoryId);
				if (category != null)
				{
					cbbCategory.DataSource = _dataCombobox;
					cbbCategory.DisplayMember = category.Result.Name;
					cbbCategory.ValueMember = category.Result.Id.ToString();
					cbbCategory.SelectedIndex = 0; // chọn mặc định là item click	
				}
			}
		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			try
			{
				var categoryId = cbbCategory.SelectedValue;
				if (categoryId is null || string.IsNullOrEmpty(categoryId.ToString()))
				{
					MessageBox.Show("Vui lòng chọn danh mục", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				if (string.IsNullOrEmpty(txtMaterialName.Text))
				{
					MessageBox.Show("Vui lòng chọn nhập tên danh mục", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				if (nbQuantity.Value <= 0)
				{
					MessageBox.Show("Số lượng lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				if (nbUnits.Value <= 0)
				{
					MessageBox.Show("Giá nhập lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				if (lstIds.Count > 1)
				{
					MessageBox.Show("Chỉ có thể sửa 1 vật liệu", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				var request = new UpdateMaterialRequestModel()
				{
					Id = lstIds.First(),
					Name = txtMaterialName.Text,
					CategoryId = Convert.ToInt32(categoryId.ToString()),
					Description = txtDescription.Text,
					Quantity = Convert.ToInt32(nbQuantity.Value),
					Units = Convert.ToInt32(nbUnits.Value),
				};
				_materialServices.UpdateMaterialsAsync(request);
				// load form khi ta
				FormLoad(this, EventArgs.Empty);
				return;
			}
			catch (Exception ex)
			{
				return;
			}
		}

		private void back_Click(object sender, EventArgs e)
		{
			var mainForm = new MainForm(_currentUser);
			mainForm.Show();
			lstIds.Clear();
			this.Close();
		}
	}
}
