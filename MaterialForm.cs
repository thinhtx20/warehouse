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
        private MaterialCategoryRespone _dataMaterialCombobox = new MaterialCategoryRespone();
        private List<MaterialResponeMessage> _materialData = new List<MaterialResponeMessage>();
        private MaterialByIdResponeMessage _materialDataById = new MaterialByIdResponeMessage();
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
        private async void btnAdd_Click(object sender, EventArgs e)
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
                    MessageBox.Show("Vui lòng nhập tên vật tư", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                // Kiểm tra tên vật tư đã tồn tại chưa (khi thêm)
                var materialName = txtMaterialName.Text.Trim();
                var isExists = await _materialServices.IsMaterialNameExistsAsync(materialName);
                if (isExists)
                {
                    MessageBox.Show("Tên vật tư đã tồn tại. Vui lòng nhập tên khác.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var request = new CreatedMaterialRequestModel()
                {
                    Name = materialName,
                    CategoryId = Convert.ToInt32(categoryId.ToString()),
                    Description = txtDescription.Text,
                    Quantity = Convert.ToInt32(nbQuantity.Value),
                    Units = Convert.ToInt32(nbUnits.Value),
                };
                var result = await _materialServices.CreatedMaterialsAsync(request);
                if (result)
                {
                    MessageBox.Show("Thêm vật tư thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Load lại dữ liệu mới nhất
                    await LoadData();
                    LoadCombobox();
                    LoadGirdView();
                    // Clear form
                    btnRefresh_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Thêm vật tư thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtDescription.Clear();
            txtMaterialName.Clear();
            cbbCategory.SelectedIndex = -1;
            nbQuantity.Value = 0;
            nbUnits.Value = 0;
            lstIds.Clear();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            // Lấy danh sách ID từ các checkbox đã được check
            var selectedIds = new List<int>();
            foreach (DataGridViewRow row in dgvMaterials.Rows)
            {
                if (row.IsNewRow) continue;
                var checkboxCell = row.Cells["materialCick"] as DataGridViewCheckBoxCell;
                if (checkboxCell != null && checkboxCell.Value != null && (bool)checkboxCell.Value == true)
                {
                    var materialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value);
                    if (materialId > 0)
                    {
                        selectedIds.Add(materialId);
                    }
                }
            }

            if (!selectedIds.Any())
            {
                MessageBox.Show("Vui lòng chọn vật tư cần xóa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xác nhận xóa
            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa {selectedIds.Count} vật tư đã chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            // Thực hiện xóa
            var deleteResult = await _materialServices.DeleteMaterialsAsync(selectedIds);
            if (deleteResult)
            {
                MessageBox.Show("Xóa vật tư thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lstIds.Clear();
                // Load lại form
                await LoadData();
                LoadCombobox();
                LoadGirdView();
            }
            else
            {
                MessageBox.Show("Xóa vật tư thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void dgvMaterials_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Chống click header hoặc row lỗi
            if (e.RowIndex < 0) return;
            _materialDataById = null;

            // Lấy ID vật liệu từ cell
            var cellValue = Convert.ToInt32(dgvMaterials.Rows[e.RowIndex].Cells["materialIdDataGridViewTextBoxColumn"].Value);
            if (cellValue == null) return;

            // Kiểm tra xem cột checkbox là cột "materialCick"
            if (dgvMaterials.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn &&
                dgvMaterials.Columns[e.ColumnIndex].Name == "materialCick")
            {
                var cell = dgvMaterials.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                if (cell != null)
                {
                    // Toggle giá trị
                    bool currentValue = (bool?)(cell.Value) ?? false;
                    cell.Value = !currentValue;

                    // Commit để hiển thị ngay
                    dgvMaterials.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                return; // Không load form khi chỉ click checkbox
            }

            // Set lstIds để biết đang sửa vật tư nào (chỉ 1 item)
            lstIds.Clear();
            lstIds.Add(cellValue);

            // Lấy vật liệu theo ID để hiển thị thông tin
            _materialDataById = await _materialServices.MaterialById(cellValue);
            if (_materialDataById == null) return;

            txtDescription.Text = _materialDataById.Description;
            txtMaterialName.Text = _materialDataById.MaterialName;
            nbQuantity.Value = _materialDataById.Quantity;
            nbUnits.Value = _materialDataById.Unit;

            // Gán lại datasource một lần (đảm bảo combobox đã có dữ liệu)
            cbbCategory.DataSource = null;
            cbbCategory.DataSource = _dataCombobox;
            cbbCategory.DisplayMember = "Name";
            cbbCategory.ValueMember = "Id";

            // Chọn đúng category của material bằng CategoryId từ _materialDataById
            if (_materialDataById.CategoryId > 0)
            {
                cbbCategory.SelectedValue = _materialDataById.CategoryId;
            }
        }


        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!lstIds.Any())
                {
                    MessageBox.Show("Vui lòng chọn vật tư cần sửa", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (lstIds.Count > 1)
                {
                    MessageBox.Show("Chỉ có thể sửa 1 vật liệu", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var categoryId = cbbCategory.SelectedValue;
                if (categoryId is null || string.IsNullOrEmpty(categoryId.ToString()))
                {
                    MessageBox.Show("Vui lòng chọn danh mục", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(txtMaterialName.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên vật tư", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                var currentMaterialId = lstIds.First();
                var materialName = txtMaterialName.Text.Trim();

                // Kiểm tra tên vật tư đã tồn tại ở vật tư khác chưa (khi sửa, loại trừ vật tư hiện tại)
                var isExists = await _materialServices.IsMaterialNameExistsAsync(materialName, currentMaterialId);
                if (isExists)
                {
                    MessageBox.Show("Tên vật tư đã tồn tại ở vật tư khác. Vui lòng nhập tên khác.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var request = new UpdateMaterialRequestModel()
                {
                    Id = currentMaterialId,
                    Name = materialName,
                    CategoryId = Convert.ToInt32(categoryId.ToString()),
                    Description = txtDescription.Text,
                    Quantity = Convert.ToInt32(nbQuantity.Value),
                    Units = Convert.ToInt32(nbUnits.Value),
                };
                var result = await _materialServices.UpdateMaterialsAsync(request);
                if (result)
                {
                    MessageBox.Show("Sửa vật tư thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lstIds.Clear();
                    // Load lại dữ liệu mới nhất
                    await LoadData();
                    LoadCombobox();
                    LoadGirdView();
                    // Clear form
                    btnRefresh_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Sửa vật tư thất bại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            var mainForm = new MainForm(_currentUser);
            mainForm.Show();
            lstIds.Clear();
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
