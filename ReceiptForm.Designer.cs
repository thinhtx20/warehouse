namespace Inventory_manager
{
	partial class ReceiptForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label lblWarehouse;
		private System.Windows.Forms.ComboBox cbWarehouse;
		private System.Windows.Forms.Label lblCreatedAt;
		private System.Windows.Forms.DateTimePicker dtCreatedAt;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Button btnExportExcel;
		private System.Windows.Forms.DataGridView dgvReceipts;
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			lblWarehouse = new Label();
			cbWarehouse = new ComboBox();
			materialResponeMessageBindingSource = new BindingSource(components);
			lblCreatedAt = new Label();
			dtCreatedAt = new DateTimePicker();
			lblDescription = new Label();
			txtDescription = new TextBox();
			btnAdd = new Button();
			btnUpdate = new Button();
			btnDelete = new Button();
			btnRefresh = new Button();
			btnExportExcel = new Button();
			dgvReceipts = new DataGridView();
			groupBox1 = new GroupBox();
			dgvListReceipt = new DataGridView();
			listReceiptResponeMessageBindingSource = new BindingSource(components);
			groupBox2 = new GroupBox();
			btnBack = new Button();
			cbDgvReceiptForm = new DataGridViewCheckBoxColumn();
			materialIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			materialNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			unitDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			categoryNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			Quantity = new DataGridViewTextBoxColumn();
			QuantityReceipt = new DataGridViewTextBoxColumn();
			cbReceipt = new DataGridViewCheckBoxColumn();
			receiptIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			warehouseNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			warehouseDescriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			totalMaterialDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			CreatedBy = new DataGridViewTextBoxColumn();
			CreatedAt = new DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)materialResponeMessageBindingSource).BeginInit();
			((System.ComponentModel.ISupportInitialize)dgvReceipts).BeginInit();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvListReceipt).BeginInit();
			((System.ComponentModel.ISupportInitialize)listReceiptResponeMessageBindingSource).BeginInit();
			groupBox2.SuspendLayout();
			SuspendLayout();
			// 
			// lblWarehouse
			// 
			lblWarehouse.AutoSize = true;
			lblWarehouse.Location = new Point(23, 20);
			lblWarehouse.Name = "lblWarehouse";
			lblWarehouse.Size = new Size(38, 20);
			lblWarehouse.TabIndex = 2;
			lblWarehouse.Text = "Kho:";
			// 
			// cbWarehouse
			// 
			cbWarehouse.DataSource = materialResponeMessageBindingSource;
			cbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
			cbWarehouse.Location = new Point(137, 16);
			cbWarehouse.Margin = new Padding(3, 4, 3, 4);
			cbWarehouse.Name = "cbWarehouse";
			cbWarehouse.Size = new Size(228, 28);
			cbWarehouse.TabIndex = 3;
			// 
			// materialResponeMessageBindingSource
			// 
			materialResponeMessageBindingSource.DataSource = typeof(dto.Response.MaterialResponeMessage);
			// 
			// lblCreatedAt
			// 
			lblCreatedAt.AutoSize = true;
			lblCreatedAt.Location = new Point(23, 87);
			lblCreatedAt.Name = "lblCreatedAt";
			lblCreatedAt.Size = new Size(73, 20);
			lblCreatedAt.TabIndex = 4;
			lblCreatedAt.Text = "Ngày tạo:";
			// 
			// dtCreatedAt
			// 
			dtCreatedAt.CustomFormat = "dd/MM/yyyy";
			dtCreatedAt.Format = DateTimePickerFormat.Custom;
			dtCreatedAt.Location = new Point(137, 76);
			dtCreatedAt.Margin = new Padding(3, 4, 3, 4);
			dtCreatedAt.Name = "dtCreatedAt";
			dtCreatedAt.Size = new Size(228, 27);
			dtCreatedAt.TabIndex = 5;
			// 
			// lblDescription
			// 
			lblDescription.AutoSize = true;
			lblDescription.Location = new Point(23, 136);
			lblDescription.Name = "lblDescription";
			lblDescription.Size = new Size(51, 20);
			lblDescription.TabIndex = 6;
			lblDescription.Text = "Mô tả:";
			// 
			// txtDescription
			// 
			txtDescription.Location = new Point(137, 133);
			txtDescription.Margin = new Padding(3, 4, 3, 4);
			txtDescription.Multiline = true;
			txtDescription.Name = "txtDescription";
			txtDescription.Size = new Size(308, 152);
			txtDescription.TabIndex = 7;
			// 
			// btnAdd
			// 
			btnAdd.Location = new Point(23, 293);
			btnAdd.Margin = new Padding(3, 4, 3, 4);
			btnAdd.Name = "btnAdd";
			btnAdd.Size = new Size(91, 40);
			btnAdd.TabIndex = 8;
			btnAdd.Text = "Thêm";
			btnAdd.Click += btnAdd_Click;
			// 
			// btnUpdate
			// 
			btnUpdate.Location = new Point(126, 293);
			btnUpdate.Margin = new Padding(3, 4, 3, 4);
			btnUpdate.Name = "btnUpdate";
			btnUpdate.Size = new Size(91, 40);
			btnUpdate.TabIndex = 9;
			btnUpdate.Text = "Sửa";
			btnUpdate.Click += btnUpdate_Click;
			// 
			// btnDelete
			// 
			btnDelete.Location = new Point(229, 293);
			btnDelete.Margin = new Padding(3, 4, 3, 4);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new Size(91, 40);
			btnDelete.TabIndex = 10;
			btnDelete.Text = "Xóa";
			btnDelete.Click += btnDelete_Click;
			// 
			// btnRefresh
			// 
			btnRefresh.Location = new Point(331, 293);
			btnRefresh.Margin = new Padding(3, 4, 3, 4);
			btnRefresh.Name = "btnRefresh";
			btnRefresh.Size = new Size(114, 40);
			btnRefresh.TabIndex = 11;
			btnRefresh.Text = "Làm mới";
			btnRefresh.Click += btnRefresh_Click;
			// 
			// btnExportExcel
			// 
			btnExportExcel.BackColor = Color.FromArgb(76, 175, 80);
			btnExportExcel.ForeColor = Color.White;
			btnExportExcel.Location = new Point(451, 293);
			btnExportExcel.Margin = new Padding(3, 4, 3, 4);
			btnExportExcel.Name = "btnExportExcel";
			btnExportExcel.Size = new Size(114, 40);
			btnExportExcel.TabIndex = 16;
			btnExportExcel.Text = "Xuất Excel";
			btnExportExcel.UseVisualStyleBackColor = false;
			btnExportExcel.Click += btnExportExcel_Click;
			// 
			// dgvReceipts
			// 
			dgvReceipts.AutoGenerateColumns = false;
			dgvReceipts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dgvReceipts.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			dgvReceipts.ColumnHeadersHeight = 29;
			dgvReceipts.Columns.AddRange(new DataGridViewColumn[] { cbDgvReceiptForm, materialIdDataGridViewTextBoxColumn, materialNameDataGridViewTextBoxColumn, unitDataGridViewTextBoxColumn, categoryNameDataGridViewTextBoxColumn, Quantity, QuantityReceipt });
			dgvReceipts.DataSource = materialResponeMessageBindingSource;
			dgvReceipts.Dock = DockStyle.Fill;
			dgvReceipts.Location = new Point(3, 23);
			dgvReceipts.Margin = new Padding(3, 4, 3, 4);
			dgvReceipts.Name = "dgvReceipts";
			dgvReceipts.RowHeadersWidth = 51;
			dgvReceipts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgvReceipts.Size = new Size(1123, 454);
			dgvReceipts.TabIndex = 12;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(dgvListReceipt);
			groupBox1.Location = new Point(460, 76);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(676, 317);
			groupBox1.TabIndex = 13;
			groupBox1.TabStop = false;
			groupBox1.Text = "Danh sách phiếu nhập hàng";
			// 
			// dgvListReceipt
			// 
			dgvListReceipt.AutoGenerateColumns = false;
			dgvListReceipt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dgvListReceipt.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvListReceipt.Columns.AddRange(new DataGridViewColumn[] { cbReceipt, receiptIDDataGridViewTextBoxColumn, warehouseNameDataGridViewTextBoxColumn, warehouseDescriptionDataGridViewTextBoxColumn, totalMaterialDataGridViewTextBoxColumn, dataGridViewTextBoxColumn1, CreatedBy, CreatedAt });
			dgvListReceipt.DataSource = listReceiptResponeMessageBindingSource;
			dgvListReceipt.Dock = DockStyle.Fill;
			dgvListReceipt.Location = new Point(3, 23);
			dgvListReceipt.Name = "dgvListReceipt";
			dgvListReceipt.RowHeadersWidth = 51;
			dgvListReceipt.Size = new Size(670, 291);
			dgvListReceipt.TabIndex = 0;
			dgvListReceipt.CellClick += dgvListReceipt_CellClick;
			// 
			// listReceiptResponeMessageBindingSource
			// 
			listReceiptResponeMessageBindingSource.DataSource = typeof(dto.Response.ListReceiptResponeMessage);
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(dgvReceipts);
			groupBox2.Location = new Point(7, 394);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new Size(1129, 480);
			groupBox2.TabIndex = 14;
			groupBox2.TabStop = false;
			groupBox2.Text = "Danh sách vật liệu";
			// 
			// btnBack
			// 
			btnBack.Location = new Point(1042, 29);
			btnBack.Margin = new Padding(3, 4, 3, 4);
			btnBack.Name = "btnBack";
			btnBack.Size = new Size(91, 40);
			btnBack.TabIndex = 15;
			btnBack.Text = "Quay lại";
			btnBack.Click += btnBack_Click;
			// 
			// cbDgvReceiptForm
			// 
			cbDgvReceiptForm.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			cbDgvReceiptForm.DataPropertyName = "Select";
			cbDgvReceiptForm.FillWeight = 126.903557F;
			cbDgvReceiptForm.HeaderText = "Chọn";
			cbDgvReceiptForm.MinimumWidth = 6;
			cbDgvReceiptForm.Name = "cbDgvReceiptForm";
			cbDgvReceiptForm.Resizable = DataGridViewTriState.True;
			cbDgvReceiptForm.SortMode = DataGridViewColumnSortMode.Automatic;
			cbDgvReceiptForm.Width = 50;
			// 
			// materialIdDataGridViewTextBoxColumn
			// 
			materialIdDataGridViewTextBoxColumn.DataPropertyName = "MaterialId";
			materialIdDataGridViewTextBoxColumn.FillWeight = 93.27411F;
			materialIdDataGridViewTextBoxColumn.HeaderText = "ID";
			materialIdDataGridViewTextBoxColumn.MinimumWidth = 6;
			materialIdDataGridViewTextBoxColumn.Name = "materialIdDataGridViewTextBoxColumn";
			// 
			// materialNameDataGridViewTextBoxColumn
			// 
			materialNameDataGridViewTextBoxColumn.DataPropertyName = "MaterialName";
			materialNameDataGridViewTextBoxColumn.FillWeight = 93.27411F;
			materialNameDataGridViewTextBoxColumn.HeaderText = "Tên";
			materialNameDataGridViewTextBoxColumn.MinimumWidth = 6;
			materialNameDataGridViewTextBoxColumn.Name = "materialNameDataGridViewTextBoxColumn";
			// 
			// unitDataGridViewTextBoxColumn
			// 
			unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
			unitDataGridViewTextBoxColumn.FillWeight = 93.27411F;
			unitDataGridViewTextBoxColumn.HeaderText = "Giá nhập";
			unitDataGridViewTextBoxColumn.MinimumWidth = 6;
			unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
			// 
			// categoryNameDataGridViewTextBoxColumn
			// 
			categoryNameDataGridViewTextBoxColumn.DataPropertyName = "CategoryName";
			categoryNameDataGridViewTextBoxColumn.FillWeight = 93.27411F;
			categoryNameDataGridViewTextBoxColumn.HeaderText = "Danh mục";
			categoryNameDataGridViewTextBoxColumn.MinimumWidth = 6;
			categoryNameDataGridViewTextBoxColumn.Name = "categoryNameDataGridViewTextBoxColumn";
			// 
			// Quantity
			// 
			Quantity.DataPropertyName = "Quantity";
			Quantity.HeaderText = "Số lượng trong kho";
			Quantity.MinimumWidth = 6;
			Quantity.Name = "Quantity";
			Quantity.ReadOnly = true;
			// 
			// QuantityReceipt
			// 
			QuantityReceipt.HeaderText = "Số lượng";
			QuantityReceipt.MinimumWidth = 6;
			QuantityReceipt.Name = "QuantityReceipt";
			// 
			// cbReceipt
			// 
			cbReceipt.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			cbReceipt.HeaderText = "";
			cbReceipt.MinimumWidth = 6;
			cbReceipt.Name = "cbReceipt";
			cbReceipt.Width = 50;
			// 
			// receiptIDDataGridViewTextBoxColumn
			// 
			receiptIDDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
			receiptIDDataGridViewTextBoxColumn.DataPropertyName = "ReceiptID";
			receiptIDDataGridViewTextBoxColumn.HeaderText = "ID";
			receiptIDDataGridViewTextBoxColumn.MinimumWidth = 6;
			receiptIDDataGridViewTextBoxColumn.Name = "receiptIDDataGridViewTextBoxColumn";
			receiptIDDataGridViewTextBoxColumn.Width = 53;
			// 
			// warehouseNameDataGridViewTextBoxColumn
			// 
			warehouseNameDataGridViewTextBoxColumn.DataPropertyName = "WarehouseName";
			warehouseNameDataGridViewTextBoxColumn.HeaderText = "Kho";
			warehouseNameDataGridViewTextBoxColumn.MinimumWidth = 6;
			warehouseNameDataGridViewTextBoxColumn.Name = "warehouseNameDataGridViewTextBoxColumn";
			// 
			// warehouseDescriptionDataGridViewTextBoxColumn
			// 
			warehouseDescriptionDataGridViewTextBoxColumn.DataPropertyName = "WarehouseDescription";
			warehouseDescriptionDataGridViewTextBoxColumn.HeaderText = "Nội dung phiếu";
			warehouseDescriptionDataGridViewTextBoxColumn.MinimumWidth = 6;
			warehouseDescriptionDataGridViewTextBoxColumn.Name = "warehouseDescriptionDataGridViewTextBoxColumn";
			// 
			// totalMaterialDataGridViewTextBoxColumn
			// 
			totalMaterialDataGridViewTextBoxColumn.DataPropertyName = "TotalMaterial";
			totalMaterialDataGridViewTextBoxColumn.HeaderText = "Số lượng vật tư";
			totalMaterialDataGridViewTextBoxColumn.MinimumWidth = 6;
			totalMaterialDataGridViewTextBoxColumn.Name = "totalMaterialDataGridViewTextBoxColumn";
			// 
			// dataGridViewTextBoxColumn1
			// 
			dataGridViewTextBoxColumn1.DataPropertyName = "WarehouseID";
			dataGridViewTextBoxColumn1.HeaderText = "WarehouseID";
			dataGridViewTextBoxColumn1.MinimumWidth = 6;
			dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			dataGridViewTextBoxColumn1.Visible = false;
			// 
			// CreatedBy
			// 
			CreatedBy.DataPropertyName = "CreatedBy";
			CreatedBy.HeaderText = "Người tạo phiếu";
			CreatedBy.MinimumWidth = 6;
			CreatedBy.Name = "CreatedBy";
			// 
			// CreatedAt
			// 
			CreatedAt.DataPropertyName = "CreatedAt";
			CreatedAt.HeaderText = "Ngày tạo";
			CreatedAt.MinimumWidth = 6;
			CreatedAt.Name = "CreatedAt";
			// 
			// ReceiptForm
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1166, 885);
			Controls.Add(btnBack);
			Controls.Add(groupBox2);
			Controls.Add(groupBox1);
			Controls.Add(lblWarehouse);
			Controls.Add(cbWarehouse);
			Controls.Add(lblCreatedAt);
			Controls.Add(dtCreatedAt);
			Controls.Add(lblDescription);
			Controls.Add(txtDescription);
			Controls.Add(btnAdd);
			Controls.Add(btnUpdate);
			Controls.Add(btnDelete);
			Controls.Add(btnExportExcel);
			Controls.Add(btnRefresh);
			Margin = new Padding(3, 4, 3, 4);
			Name = "ReceiptForm";
			Text = "Quản lý phiếu nhập hàng";
			((System.ComponentModel.ISupportInitialize)materialResponeMessageBindingSource).EndInit();
			((System.ComponentModel.ISupportInitialize)dgvReceipts).EndInit();
			groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvListReceipt).EndInit();
			((System.ComponentModel.ISupportInitialize)listReceiptResponeMessageBindingSource).EndInit();
			groupBox2.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private BindingSource materialResponeMessageBindingSource;
		private DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private DataGridView dgvListReceipt;
		private BindingSource listReceiptResponeMessageBindingSource;
		private Button btnBack;
		private DataGridViewCheckBoxColumn cbReceipt;
		private DataGridViewTextBoxColumn receiptIDDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn warehouseNameDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn warehouseDescriptionDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn totalMaterialDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn CreatedBy;
		private DataGridViewTextBoxColumn CreatedAt;
		private DataGridViewCheckBoxColumn cbDgvReceiptForm;
		private DataGridViewTextBoxColumn materialIdDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn materialNameDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn categoryNameDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn Quantity;
		private DataGridViewTextBoxColumn Column1;
		private DataGridViewTextBoxColumn QuantityReceipt;
	}
}