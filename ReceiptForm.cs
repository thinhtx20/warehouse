using Inventory_manager.dto.Request;
using Inventory_manager.dto.Response;
using Inventory_manager.Models;
using Inventory_manager.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;

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
            UpdateButtonStates();
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
            
            // Format cột CreatedAt để hiển thị theo định dạng 24h
            if (dgvListReceipt.Columns["CreatedAt"] != null)
            {
                dgvListReceipt.Columns["CreatedAt"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
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
                
                // Kiểm tra có vật tư nào được chọn không
                bool hasSelectedMaterial = false;
                foreach (DataGridViewRow row in dgvReceipts.Rows)
                {
                    if (row.IsNewRow) continue;

                    var selected = row.Cells["cbDgvReceiptForm"].Value;
                    if (selected != null && (bool)selected == true)
                    {
                        hasSelectedMaterial = true;
                        var quantityReceiptValue = row.Cells["QuantityReceipt"].Value;
                        if (quantityReceiptValue == null || string.IsNullOrEmpty(quantityReceiptValue.ToString()))
                        {
                            MessageBox.Show("Vui lòng nhập số lượng cho vật tư đã chọn", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var materialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value);
                        var quantityReceipt = Convert.ToInt32(quantityReceiptValue);

                        // Kiểm tra số lượng vật tư nhập phải lớn hơn 0
                        if (quantityReceipt <= 0)
                        {
                            var materialName = row.Cells["materialNameDataGridViewTextBoxColumn"].Value?.ToString() ?? "vật tư";
                            MessageBox.Show($"Số lượng vật tư '{materialName}' nhập phải lớn hơn 0", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Kiểm tra số lượng nhập không được vượt quá số lượng vật tư hiện có
                        var material = _materialData.FirstOrDefault(m => m.MaterialId == materialId);
                        if (material != null && quantityReceipt > material.QuantitySL)
                        {
                            MessageBox.Show($"Vật tư '{material.MaterialName}' có số lượng không được quá số lượng vật tư hiện có ({material.QuantitySL})", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        body.Items.Add(new ReceiptItem()
                        {
                            MaterialId = materialId,
                            Quantity = quantityReceipt,
                            UnitPrice = Convert.ToDecimal(row.Cells["unitDataGridViewTextBoxColumn"].Value)
                        });
                    }
                }
                
                // Kiểm tra có ít nhất một vật tư được chọn
                if (!hasSelectedMaterial || !body.Items.Any())
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một vật tư", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                _inventoryService.CreateReceipt(body);
                MessageBox.Show("Thêm phiếu thành công", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // load form khi ta
                ReceiptForm_Load(this, EventArgs.Empty);
                UpdateButtonStates();
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
        private void UpdateButtonStates()
        {
            int selectedCount = lstIds?.Count ?? 0;

            if (selectedCount == 1)
            {
                // Chọn 1 item: enable nút sửa và xóa, disable nút thêm
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            else if (selectedCount > 1)
            {
                // Chọn > 1 item: disable nút thêm và nút sửa, chỉ enable nút xóa
                btnAdd.Enabled = false;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = true;
            }
            else
            {
                // Không chọn gì (0 item): enable nút thêm, disable nút sửa và xóa
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
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
                UpdateButtonStates();
                return;
            }

            var idClick = Convert.ToInt32(dgvListReceipt.Rows[e.RowIndex].Cells["receiptIDDataGridViewTextBoxColumn"].Value);
            lstIds.Clear();
            lstIds.Add(idClick);
            UpdateButtonStates();

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

                        var materialId = Convert.ToInt32(row.Cells["materialIdDataGridViewTextBoxColumn"].Value);
                        var quantityReceipt = Convert.ToInt32(quantityReceiptValue);

                        // Kiểm tra số lượng nhập không được vượt quá số lượng vật tư hiện có
                        var material = _materialData.FirstOrDefault(m => m.MaterialId == materialId);
                        if (material != null && quantityReceipt > material.QuantitySL)
                        {
                            MessageBox.Show($"Vật tư '{material.MaterialName}' có số lượng không được quá số lượng vật tư hiện có ({material.QuantitySL})", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        body.Items.Add(new ReceiptItem()
                        {
                            MaterialId = materialId,
                            Quantity = quantityReceipt,
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
                    UpdateButtonStates();
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
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (_receiptData == null || !_receiptData.Any())
                {
                    MessageBox.Show("Không có dữ liệu để xuất Excel", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.FileName = $"DanhSachPhieuNhap_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportReceiptsToExcel(_receiptData, saveFileDialog.FileName);
                        MessageBox.Show("Xuất Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất Excel: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportReceiptsToExcel(List<ListReceiptResponeMessage> receipts, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách phiếu nhập");

                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã phiếu";
                worksheet.Cells[1, 3].Value = "Tên kho";
                worksheet.Cells[1, 4].Value = "Mô tả";
                worksheet.Cells[1, 5].Value = "Tổng số lượng";
                worksheet.Cells[1, 6].Value = "Người tạo";
                worksheet.Cells[1, 7].Value = "Ngày tạo";

                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    // Thêm border cho header
                    range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                }

                int row = 2;
                int stt = 1;
                foreach (var receipt in receipts)
                {
                    worksheet.Cells[row, 1].Value = stt;
                    worksheet.Cells[row, 2].Value = receipt.ReceiptID;
                    worksheet.Cells[row, 3].Value = receipt.WarehouseName ?? "";
                    worksheet.Cells[row, 4].Value = receipt.WarehouseDescription ?? "";
                    worksheet.Cells[row, 5].Value = receipt.TotalMaterial;
                    worksheet.Cells[row, 6].Value = receipt.CreatedBy ?? "";
                    worksheet.Cells[row, 7].Value = receipt.CreatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "";

                    worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // Thêm border cho từng dòng dữ liệu
                    using (var dataRange = worksheet.Cells[row, 1, row, 7])
                    {
                        dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    row++;
                    stt++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                worksheet.Column(1).Width = 8;
                worksheet.Column(2).Width = 12;
                worksheet.Column(3).Width = 20;
                worksheet.Column(4).Width = 30;
                worksheet.Column(5).Width = 15;
                worksheet.Column(6).Width = 20;
                worksheet.Column(7).Width = 18;

                worksheet.Row(1).Height = 25;

                package.SaveAs(new FileInfo(filePath));
            }
        }

    }
}
