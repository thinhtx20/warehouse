using Inventory_manager.dto.Request;
using Inventory_manager.dto.Response;
using Inventory_manager.Models;
using Inventory_manager.Services;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;
using System.IO;

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
            lstIds.Clear();
            this.Close(); // Chỉ đóng form hiện tại, MainForm sẽ tự động hiện lại
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


        private async void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy ReceiptID từ checkbox được chọn
                int? selectedReceiptId = null;
                
                if (dgvListReceipt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để xuất Excel", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Duyệt qua tất cả các dòng trong dgvListReceipt để tìm phiếu được chọn
                foreach (DataGridViewRow row in dgvListReceipt.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Kiểm tra checkbox cbReceipt có được tick không
                    var checkboxCell = row.Cells["cbReceipt"] as DataGridViewCheckBoxCell;
                    if (checkboxCell != null && checkboxCell.Value != null)
                    {
                        bool isChecked = (bool)checkboxCell.Value;
                        if (isChecked)
                        {
                            // Lấy ReceiptID từ dòng được chọn
                            var receiptIdCell = row.Cells["receiptIDDataGridViewTextBoxColumn"];
                            if (receiptIdCell != null && receiptIdCell.Value != null)
                            {
                                if (int.TryParse(receiptIdCell.Value.ToString(), out int receiptId))
                                {
                                    selectedReceiptId = receiptId;
                                    break; // Chỉ lấy phiếu đầu tiên được chọn
                                }
                            }
                        }
                    }
                }

                // Kiểm tra có phiếu nào được chọn không
                if (!selectedReceiptId.HasValue)
                {
                    MessageBox.Show("Vui lòng chọn một phiếu nhập hàng để xuất Excel", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.FileName = $"PhieuNhapHang_{selectedReceiptId.Value}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        await ExportReceiptToExcel(selectedReceiptId.Value, saveFileDialog.FileName);
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
        /// Xuất Excel cho phiếu nhập hàng đã chọn với chi tiết vật tư và tổng tiền
        /// </summary>
        private async Task ExportReceiptToExcel(int receiptId, string filePath)
        {
            using (var package = new ExcelPackage())
            {
                using var _db = new WarehousesManagerContext();
                
                // Load chi tiết phiếu nhập hàng
                var receipt = await _db.InventoryReceipts.AsNoTracking()
                    .Include(x => x.CreatedByNavigation)
                    .Include(x => x.Warehouse)
                    .Include(x => x.InventoryReceiptDetails)
                        .ThenInclude(x => x.Material)
                    .FirstOrDefaultAsync(x => x.ReceiptId == receiptId);

                if (receipt == null)
                {
                    throw new Exception("Không tìm thấy phiếu nhập hàng");
                }

                var worksheet = package.Workbook.Worksheets.Add("Phiếu nhập hàng");

                // Header thông tin phiếu
                int currentRow = 1;
                worksheet.Cells[currentRow, 1].Value = "PHIẾU NHẬP HÀNG";
                worksheet.Cells[currentRow, 1, currentRow, 6].Merge = true;
                worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
                worksheet.Cells[currentRow, 1].Style.Font.Size = 16;
                worksheet.Cells[currentRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                currentRow += 2;

                // Thông tin phiếu
                worksheet.Cells[currentRow, 1].Value = "Mã phiếu:";
                worksheet.Cells[currentRow, 2].Value = receipt.ReceiptCode ?? "";
                worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                worksheet.Cells[currentRow, 1].Value = "Kho:";
                worksheet.Cells[currentRow, 2].Value = receipt.Warehouse?.WarehouseName ?? "";
                worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                worksheet.Cells[currentRow, 1].Value = "Ngày nhập:";
                worksheet.Cells[currentRow, 2].Value = receipt.CreatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "";
                worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                worksheet.Cells[currentRow, 1].Value = "Người tạo:";
                worksheet.Cells[currentRow, 2].Value = receipt.CreatedByNavigation?.FullName ?? "";
                worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
                currentRow++;

                if (!string.IsNullOrEmpty(receipt.Description))
                {
                    worksheet.Cells[currentRow, 1].Value = "Mô tả:";
                    worksheet.Cells[currentRow, 2].Value = receipt.Description;
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

                foreach (var detail in receipt.InventoryReceiptDetails.OrderBy(x => x.MaterialId))
                {
                    int quantity = detail.Quantity;
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
                worksheet.Column(5).Width = 18;

                worksheet.Row(1).Height = 25;

                package.SaveAs(new FileInfo(filePath));
            }
        }

    }
}
