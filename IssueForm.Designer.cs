namespace Inventory_manager
{
	partial class IssueForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			btnBack = new Button();
			listIssueResponeMessageBindingSource = new BindingSource(components);
			CreatedAt = new DataGridViewTextBoxColumn();
			CreatedBy = new DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
			totalMaterialDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			warehouseDescriptionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			warehouseNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			IssueIDDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			cbIssue = new DataGridViewCheckBoxColumn();
			dgvListIssue = new DataGridView();
			groupBox1 = new GroupBox();
			Column1 = new DataGridViewTextBoxColumn();
			Quantity = new DataGridViewTextBoxColumn();
			categoryNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			unitDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			materialNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			materialIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			cbDgvIssueForm = new DataGridViewCheckBoxColumn();
			dgvIssues = new DataGridView();
			materialResponeMessageBindingSource = new BindingSource(components);
			lblCreatedAt = new Label();
			dtCreatedAt = new DateTimePicker();
			lblDescription = new Label();
			txtDescription = new TextBox();
			btnAdd = new Button();
			btnUpdate = new Button();
			btnDelete = new Button();
			btnRefresh = new Button();
			groupBox2 = new GroupBox();
			lblWarehouse = new Label();
			cbWarehouse = new ComboBox();
			((System.ComponentModel.ISupportInitialize)listIssueResponeMessageBindingSource).BeginInit();
			((System.ComponentModel.ISupportInitialize)dgvListIssue).BeginInit();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvIssues).BeginInit();
			((System.ComponentModel.ISupportInitialize)materialResponeMessageBindingSource).BeginInit();
			groupBox2.SuspendLayout();
			SuspendLayout();
			// 
			// btnBack
			// 
			btnBack.Location = new Point(1054, 13);
			btnBack.Margin = new Padding(3, 4, 3, 4);
			btnBack.Name = "btnBack";
			btnBack.Size = new Size(91, 40);
			btnBack.TabIndex = 28;
			btnBack.Text = "Quay lại";
			// 
			// listIssueResponeMessageBindingSource
			// 
			listIssueResponeMessageBindingSource.DataSource = typeof(dto.Response.ListIssueResponseMessage);
			// 
			// CreatedAt
			// 
			CreatedAt.DataPropertyName = "CreatedAt";
			CreatedAt.HeaderText = "Ngày tạo";
			CreatedAt.MinimumWidth = 6;
			CreatedAt.Name = "CreatedAt";
			// 
			// CreatedBy
			// 
			CreatedBy.DataPropertyName = "CreatedBy";
			CreatedBy.HeaderText = "Người tạo phiếu";
			CreatedBy.MinimumWidth = 6;
			CreatedBy.Name = "CreatedBy";
			// 
			// dataGridViewTextBoxColumn1
			// 
			dataGridViewTextBoxColumn1.DataPropertyName = "WarehouseID";
			dataGridViewTextBoxColumn1.HeaderText = "WarehouseID";
			dataGridViewTextBoxColumn1.MinimumWidth = 6;
			dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			dataGridViewTextBoxColumn1.Visible = false;
			// 
			// totalMaterialDataGridViewTextBoxColumn
			// 
			totalMaterialDataGridViewTextBoxColumn.DataPropertyName = "TotalMaterial";
			totalMaterialDataGridViewTextBoxColumn.HeaderText = "Số lượng vật tư";
			totalMaterialDataGridViewTextBoxColumn.MinimumWidth = 6;
			totalMaterialDataGridViewTextBoxColumn.Name = "totalMaterialDataGridViewTextBoxColumn";
			// 
			// warehouseDescriptionDataGridViewTextBoxColumn
			// 
			warehouseDescriptionDataGridViewTextBoxColumn.DataPropertyName = "WarehouseDescription";
			warehouseDescriptionDataGridViewTextBoxColumn.HeaderText = "Nội dung phiếu";
			warehouseDescriptionDataGridViewTextBoxColumn.MinimumWidth = 6;
			warehouseDescriptionDataGridViewTextBoxColumn.Name = "warehouseDescriptionDataGridViewTextBoxColumn";
			// 
			// warehouseNameDataGridViewTextBoxColumn
			// 
			warehouseNameDataGridViewTextBoxColumn.DataPropertyName = "WarehouseName";
			warehouseNameDataGridViewTextBoxColumn.HeaderText = "Kho";
			warehouseNameDataGridViewTextBoxColumn.MinimumWidth = 6;
			warehouseNameDataGridViewTextBoxColumn.Name = "warehouseNameDataGridViewTextBoxColumn";
			// 
			// IssueIDDataGridViewTextBoxColumn
			// 
			IssueIDDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
			IssueIDDataGridViewTextBoxColumn.DataPropertyName = "IssueID";
			IssueIDDataGridViewTextBoxColumn.HeaderText = "ID";
			IssueIDDataGridViewTextBoxColumn.MinimumWidth = 6;
			IssueIDDataGridViewTextBoxColumn.Name = "issueIDDataGridViewTextBoxColumn";
			IssueIDDataGridViewTextBoxColumn.Width = 53;
			// 
			// cbIssue
			// 
			cbIssue.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			cbIssue.HeaderText = "";
			cbIssue.MinimumWidth = 6;
			cbIssue.Name = "cbIssue";
			cbIssue.Width = 50;
			// 
			// dgvListIssue
			// 
			dgvListIssue.AutoGenerateColumns = false;
			dgvListIssue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dgvListIssue.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvListIssue.Columns.AddRange(new DataGridViewColumn[] { cbIssue, IssueIDDataGridViewTextBoxColumn, warehouseNameDataGridViewTextBoxColumn, warehouseDescriptionDataGridViewTextBoxColumn, totalMaterialDataGridViewTextBoxColumn, dataGridViewTextBoxColumn1, CreatedBy, CreatedAt });
			dgvListIssue.DataSource = listIssueResponeMessageBindingSource;
			dgvListIssue.Dock = DockStyle.Fill;
			dgvListIssue.Location = new Point(3, 23);
			dgvListIssue.Name = "dgvListIssue";
			dgvListIssue.RowHeadersWidth = 51;
			dgvListIssue.Size = new Size(670, 291);
			dgvListIssue.TabIndex = 0;
			// 
			// groupBox1
			// 
			groupBox1.Controls.Add(dgvListIssue);
			groupBox1.Location = new Point(472, 60);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(676, 317);
			groupBox1.TabIndex = 26;
			groupBox1.TabStop = false;
			groupBox1.Text = "Danh sách phiếu xuất kho";
			// 
			// Column1
			// 
			Column1.HeaderText = "Số lượng";
			Column1.MinimumWidth = 6;
			Column1.Name = "Column1";
			// 
			// Quantity
			// 
			Quantity.DataPropertyName = "Quantity";
			Quantity.HeaderText = "Số lượng trong kho";
			Quantity.MinimumWidth = 6;
			Quantity.Name = "Quantity";
			// 
			// categoryNameDataGridViewTextBoxColumn
			// 
			categoryNameDataGridViewTextBoxColumn.DataPropertyName = "CategoryName";
			categoryNameDataGridViewTextBoxColumn.FillWeight = 93.27411F;
			categoryNameDataGridViewTextBoxColumn.HeaderText = "Danh mục";
			categoryNameDataGridViewTextBoxColumn.MinimumWidth = 6;
			categoryNameDataGridViewTextBoxColumn.Name = "categoryNameDataGridViewTextBoxColumn";
			// 
			// unitDataGridViewTextBoxColumn
			// 
			unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
			unitDataGridViewTextBoxColumn.FillWeight = 93.27411F;
			unitDataGridViewTextBoxColumn.HeaderText = "Giá nhập";
			unitDataGridViewTextBoxColumn.MinimumWidth = 6;
			unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
			// 
			// materialNameDataGridViewTextBoxColumn
			// 
			materialNameDataGridViewTextBoxColumn.DataPropertyName = "MaterialName";
			materialNameDataGridViewTextBoxColumn.FillWeight = 93.27411F;
			materialNameDataGridViewTextBoxColumn.HeaderText = "Tên";
			materialNameDataGridViewTextBoxColumn.MinimumWidth = 6;
			materialNameDataGridViewTextBoxColumn.Name = "materialNameDataGridViewTextBoxColumn";
			// 
			// materialIdDataGridViewTextBoxColumn
			// 
			materialIdDataGridViewTextBoxColumn.DataPropertyName = "MaterialId";
			materialIdDataGridViewTextBoxColumn.FillWeight = 93.27411F;
			materialIdDataGridViewTextBoxColumn.HeaderText = "ID";
			materialIdDataGridViewTextBoxColumn.MinimumWidth = 6;
			materialIdDataGridViewTextBoxColumn.Name = "materialIdDataGridViewTextBoxColumn";
			// 
			// cbDgvIssueForm
			// 
			cbDgvIssueForm.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			cbDgvIssueForm.DataPropertyName = "Select";
			cbDgvIssueForm.FillWeight = 126.903557F;
			cbDgvIssueForm.HeaderText = "Chọn";
			cbDgvIssueForm.MinimumWidth = 6;
			cbDgvIssueForm.Name = "cbDgvIssueForm";
			cbDgvIssueForm.Resizable = DataGridViewTriState.True;
			cbDgvIssueForm.SortMode = DataGridViewColumnSortMode.Automatic;
			cbDgvIssueForm.Width = 50;
			// 
			// dgvIssues
			// 
			dgvIssues.AutoGenerateColumns = false;
			dgvIssues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dgvIssues.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
			dgvIssues.ColumnHeadersHeight = 29;
			dgvIssues.Columns.AddRange(new DataGridViewColumn[] { cbDgvIssueForm, materialIdDataGridViewTextBoxColumn, materialNameDataGridViewTextBoxColumn, unitDataGridViewTextBoxColumn, categoryNameDataGridViewTextBoxColumn, Quantity, Column1 });
			dgvIssues.DataSource = materialResponeMessageBindingSource;
			dgvIssues.Dock = DockStyle.Fill;
			dgvIssues.Location = new Point(3, 23);
			dgvIssues.Margin = new Padding(3, 4, 3, 4);
			dgvIssues.Name = "dgvIssues";
			dgvIssues.RowHeadersWidth = 51;
			dgvIssues.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dgvIssues.Size = new Size(1123, 454);
			dgvIssues.TabIndex = 12;
			// 
			// materialResponeMessageBindingSource
			// 
			materialResponeMessageBindingSource.DataSource = typeof(dto.Response.MaterialResponeMessage);
			// 
			// lblCreatedAt
			// 
			lblCreatedAt.AutoSize = true;
			lblCreatedAt.Location = new Point(32, 106);
			lblCreatedAt.Name = "lblCreatedAt";
			lblCreatedAt.Size = new Size(73, 20);
			lblCreatedAt.TabIndex = 18;
			lblCreatedAt.Text = "Ngày tạo:";
			// 
			// dtCreatedAt
			// 
			dtCreatedAt.CustomFormat = "dd/MM/yyyy";
			dtCreatedAt.Format = DateTimePickerFormat.Custom;
			dtCreatedAt.Location = new Point(146, 95);
			dtCreatedAt.Margin = new Padding(3, 4, 3, 4);
			dtCreatedAt.Name = "dtCreatedAt";
			dtCreatedAt.Size = new Size(228, 27);
			dtCreatedAt.TabIndex = 19;
			// 
			// lblDescription
			// 
			lblDescription.AutoSize = true;
			lblDescription.Location = new Point(32, 155);
			lblDescription.Name = "lblDescription";
			lblDescription.Size = new Size(51, 20);
			lblDescription.TabIndex = 20;
			lblDescription.Text = "Mô tả:";
			// 
			// txtDescription
			// 
			txtDescription.Location = new Point(146, 152);
			txtDescription.Margin = new Padding(3, 4, 3, 4);
			txtDescription.Multiline = true;
			txtDescription.Name = "txtDescription";
			txtDescription.Size = new Size(308, 152);
			txtDescription.TabIndex = 21;
			// 
			// btnAdd
			// 
			btnAdd.Location = new Point(32, 312);
			btnAdd.Margin = new Padding(3, 4, 3, 4);
			btnAdd.Name = "btnAdd";
			btnAdd.Size = new Size(91, 40);
			btnAdd.TabIndex = 22;
			btnAdd.Text = "Thêm";
			// 
			// btnUpdate
			// 
			btnUpdate.Location = new Point(135, 312);
			btnUpdate.Margin = new Padding(3, 4, 3, 4);
			btnUpdate.Name = "btnUpdate";
			btnUpdate.Size = new Size(91, 40);
			btnUpdate.TabIndex = 23;
			btnUpdate.Text = "Sửa";
			// 
			// btnDelete
			// 
			btnDelete.Location = new Point(238, 312);
			btnDelete.Margin = new Padding(3, 4, 3, 4);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new Size(91, 40);
			btnDelete.TabIndex = 24;
			btnDelete.Text = "Xóa";
			// 
			// btnRefresh
			// 
			btnRefresh.Location = new Point(340, 312);
			btnRefresh.Margin = new Padding(3, 4, 3, 4);
			btnRefresh.Name = "btnRefresh";
			btnRefresh.Size = new Size(114, 40);
			btnRefresh.TabIndex = 25;
			btnRefresh.Text = "Làm mới";
			// 
			// groupBox2
			// 
			groupBox2.Controls.Add(dgvIssues);
			groupBox2.Location = new Point(16, 413);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new Size(1129, 480);
			groupBox2.TabIndex = 27;
			groupBox2.TabStop = false;
			groupBox2.Text = "Danh sách vật liệu";
			// 
			// lblWarehouse
			// 
			lblWarehouse.AutoSize = true;
			lblWarehouse.Location = new Point(32, 39);
			lblWarehouse.Name = "lblWarehouse";
			lblWarehouse.Size = new Size(38, 20);
			lblWarehouse.TabIndex = 16;
			lblWarehouse.Text = "Kho:";
			// 
			// cbWarehouse
			// 
			cbWarehouse.DataSource = materialResponeMessageBindingSource;
			cbWarehouse.DropDownStyle = ComboBoxStyle.DropDownList;
			cbWarehouse.Location = new Point(146, 35);
			cbWarehouse.Margin = new Padding(3, 4, 3, 4);
			cbWarehouse.Name = "cbWarehouse";
			cbWarehouse.Size = new Size(228, 28);
			cbWarehouse.TabIndex = 17;
			// 
			// IssueForm
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1172, 832);
			Controls.Add(btnBack);
			Controls.Add(groupBox1);
			Controls.Add(lblCreatedAt);
			Controls.Add(dtCreatedAt);
			Controls.Add(lblDescription);
			Controls.Add(txtDescription);
			Controls.Add(btnAdd);
			Controls.Add(btnUpdate);
			Controls.Add(btnDelete);
			Controls.Add(btnRefresh);
			Controls.Add(groupBox2);
			Controls.Add(lblWarehouse);
			Controls.Add(cbWarehouse);
			Margin = new Padding(3, 4, 3, 4);
			Name = "IssueForm";
			Text = "IssueForm";
			Load += IssueForm_Load;
			((System.ComponentModel.ISupportInitialize)listIssueResponeMessageBindingSource).EndInit();
			((System.ComponentModel.ISupportInitialize)dgvListIssue).EndInit();
			groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvIssues).EndInit();
			((System.ComponentModel.ISupportInitialize)materialResponeMessageBindingSource).EndInit();
			groupBox2.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button btnBack;
		private BindingSource listIssueResponeMessageBindingSource;
		private DataGridViewTextBoxColumn CreatedAt;
		private DataGridViewTextBoxColumn CreatedBy;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private DataGridViewTextBoxColumn totalMaterialDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn warehouseDescriptionDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn warehouseNameDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn IssueIDDataGridViewTextBoxColumn;
		private DataGridViewCheckBoxColumn cbIssue;
		private DataGridView dgvListIssue;
		private GroupBox groupBox1;
		private DataGridViewTextBoxColumn Column1;
		private DataGridViewTextBoxColumn Quantity;
		private DataGridViewTextBoxColumn categoryNameDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn materialNameDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn materialIdDataGridViewTextBoxColumn;
		private DataGridViewCheckBoxColumn cbDgvIssueForm;
		private DataGridView dgvIssues;
		private BindingSource materialResponeMessageBindingSource;
		private Label lblCreatedAt;
		private DateTimePicker dtCreatedAt;
		private Label lblDescription;
		private TextBox txtDescription;
		private Button btnAdd;
		private Button btnUpdate;
		private Button btnDelete;
		private Button btnRefresh;
		private GroupBox groupBox2;
		private Label lblWarehouse;
		private ComboBox cbWarehouse;
	}
}