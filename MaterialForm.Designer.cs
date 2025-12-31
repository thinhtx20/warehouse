using Inventory_manager.Services;

namespace Inventory_manager
{
	partial class MaterialForm
	{
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.GroupBox groupBoxMaterials;
		private System.Windows.Forms.DataGridView dgvMaterials;
		private System.Windows.Forms.TextBox txtMaterialName;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblUnit;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            groupBoxMaterials = new GroupBox();
            dgvMaterials = new DataGridView();
            materialCick = new DataGridViewCheckBoxColumn();
            materialIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            materialNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            unitDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            weightDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            categoryNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            materialResponeMessageBindingSource = new BindingSource(components);
            lblName = new Label();
            lblUnit = new Label();
            txtMaterialName = new TextBox();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            back = new Button();
            groupBox1 = new GroupBox();
            nbUnits = new NumericUpDown();
            nbQuantity = new NumericUpDown();
            cbbCategory = new ComboBox();
            label3 = new Label();
            txtDescription = new TextBox();
            label2 = new Label();
            label1 = new Label();
            toolTip1 = new ToolTip(components);
            txtSearch = new TextBox();
            txtlablesearch = new Label();
            groupBoxMaterials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMaterials).BeginInit();
            ((System.ComponentModel.ISupportInitialize)materialResponeMessageBindingSource).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nbUnits).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nbQuantity).BeginInit();
            SuspendLayout();
            // 
            // groupBoxMaterials
            // 
            groupBoxMaterials.Controls.Add(dgvMaterials);
            groupBoxMaterials.Location = new Point(12, 268);
            groupBoxMaterials.Name = "groupBoxMaterials";
            groupBoxMaterials.Size = new Size(835, 480);
            groupBoxMaterials.TabIndex = 0;
            groupBoxMaterials.TabStop = false;
            groupBoxMaterials.Text = "Danh sách vật tư";
            // 
            // dgvMaterials
            // 
            dgvMaterials.AutoGenerateColumns = false;
            dgvMaterials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMaterials.ColumnHeadersHeight = 29;
            dgvMaterials.Columns.AddRange(new DataGridViewColumn[] { materialCick, materialIdDataGridViewTextBoxColumn, materialNameDataGridViewTextBoxColumn, unitDataGridViewTextBoxColumn, weightDataGridViewTextBoxColumn, categoryNameDataGridViewTextBoxColumn });
            dgvMaterials.DataSource = materialResponeMessageBindingSource;
            dgvMaterials.Dock = DockStyle.Fill;
            dgvMaterials.Location = new Point(3, 35);
            dgvMaterials.Name = "dgvMaterials";
            dgvMaterials.ReadOnly = true;
            dgvMaterials.RowHeadersWidth = 51;
            dgvMaterials.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaterials.Size = new Size(829, 442);
            dgvMaterials.TabIndex = 0;
            dgvMaterials.CellClick += dgvMaterials_CellClick;
            dgvMaterials.CellContentClick += dgvMaterials_CellContentClick;
            // 
            // materialCick
            // 
            materialCick.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            materialCick.HeaderText = "Chọn";
            materialCick.MinimumWidth = 10;
            materialCick.Name = "materialCick";
            materialCick.ReadOnly = true;
            materialCick.Width = 50;
            // 
            // materialIdDataGridViewTextBoxColumn
            // 
            materialIdDataGridViewTextBoxColumn.DataPropertyName = "MaterialId";
            materialIdDataGridViewTextBoxColumn.HeaderText = "ID";
            materialIdDataGridViewTextBoxColumn.MinimumWidth = 10;
            materialIdDataGridViewTextBoxColumn.Name = "materialIdDataGridViewTextBoxColumn";
            materialIdDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // materialNameDataGridViewTextBoxColumn
            // 
            materialNameDataGridViewTextBoxColumn.DataPropertyName = "MaterialName";
            materialNameDataGridViewTextBoxColumn.HeaderText = "Tên";
            materialNameDataGridViewTextBoxColumn.MinimumWidth = 10;
            materialNameDataGridViewTextBoxColumn.Name = "materialNameDataGridViewTextBoxColumn";
            materialNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // unitDataGridViewTextBoxColumn
            // 
            unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
            unitDataGridViewTextBoxColumn.HeaderText = "Giá nhập";
            unitDataGridViewTextBoxColumn.MinimumWidth = 10;
            unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
            unitDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // weightDataGridViewTextBoxColumn
            // 
            weightDataGridViewTextBoxColumn.DataPropertyName = "QuantitySL";
            weightDataGridViewTextBoxColumn.HeaderText = "Số lượng";
            weightDataGridViewTextBoxColumn.MinimumWidth = 10;
            weightDataGridViewTextBoxColumn.Name = "weightDataGridViewTextBoxColumn";
            weightDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // categoryNameDataGridViewTextBoxColumn
            // 
            categoryNameDataGridViewTextBoxColumn.DataPropertyName = "CategoryName";
            categoryNameDataGridViewTextBoxColumn.HeaderText = "Danh mục";
            categoryNameDataGridViewTextBoxColumn.MinimumWidth = 10;
            categoryNameDataGridViewTextBoxColumn.Name = "categoryNameDataGridViewTextBoxColumn";
            categoryNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // materialResponeMessageBindingSource
            // 
            materialResponeMessageBindingSource.DataSource = typeof(dto.Response.MaterialResponeMessage);
            // 
            // lblName
            // 
            lblName.Location = new Point(6, 63);
            lblName.Name = "lblName";
            lblName.Size = new Size(80, 25);
            lblName.TabIndex = 2;
            lblName.Text = "Số lượng:";
            // 
            // lblUnit
            // 
            lblUnit.Location = new Point(6, 103);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(80, 25);
            lblUnit.TabIndex = 3;
            lblUnit.Text = "Giá nhập:";
            // 
            // txtMaterialName
            // 
            txtMaterialName.Location = new Point(92, 21);
            txtMaterialName.Name = "txtMaterialName";
            txtMaterialName.Size = new Size(239, 39);
            txtMaterialName.TabIndex = 5;
            // 
            // btnAdd
            // 
            btnAdd.BackColor = Color.FromArgb(128, 255, 128);
            btnAdd.Location = new Point(421, 23);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(100, 40);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.FromArgb(255, 255, 128);
            btnUpdate.Location = new Point(421, 86);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(100, 40);
            btnUpdate.TabIndex = 8;
            btnUpdate.Text = "Sửa";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.FromArgb(255, 128, 128);
            btnDelete.Location = new Point(421, 132);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(100, 40);
            btnDelete.TabIndex = 9;
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // back
            // 
            back.Location = new Point(744, 16);
            back.Name = "back";
            back.Size = new Size(100, 40);
            back.TabIndex = 11;
            back.Text = "Quay lại";
            back.Click += back_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(nbUnits);
            groupBox1.Controls.Add(nbQuantity);
            groupBox1.Controls.Add(cbbCategory);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtDescription);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(btnDelete);
            groupBox1.Controls.Add(lblName);
            groupBox1.Controls.Add(btnUpdate);
            groupBox1.Controls.Add(lblUnit);
            groupBox1.Controls.Add(btnAdd);
            groupBox1.Controls.Add(txtMaterialName);
            groupBox1.Location = new Point(24, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(565, 237);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            // 
            // nbUnits
            // 
            nbUnits.Location = new Point(92, 101);
            nbUnits.Maximum = new decimal(new int[] { -469762048, -590869294, 5421010, 0 });
            nbUnits.Name = "nbUnits";
            nbUnits.Size = new Size(239, 39);
            nbUnits.TabIndex = 18;
            // 
            // nbQuantity
            // 
            nbQuantity.Location = new Point(92, 63);
            nbQuantity.Maximum = new decimal(new int[] { 1661992960, 1808227885, 5, 0 });
            nbQuantity.Name = "nbQuantity";
            nbQuantity.Size = new Size(239, 39);
            nbQuantity.TabIndex = 12;
            // 
            // cbbCategory
            // 
            cbbCategory.FormattingEnabled = true;
            cbbCategory.Location = new Point(92, 193);
            cbbCategory.Name = "cbbCategory";
            cbbCategory.Size = new Size(237, 40);
            cbbCategory.TabIndex = 17;
            // 
            // label3
            // 
            label3.Location = new Point(6, 193);
            label3.Name = "label3";
            label3.Size = new Size(80, 25);
            label3.TabIndex = 16;
            label3.Text = "Danh mục: ";
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(92, 142);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(237, 42);
            txtDescription.TabIndex = 15;
            // 
            // label2
            // 
            label2.Location = new Point(6, 148);
            label2.Name = "label2";
            label2.Size = new Size(80, 25);
            label2.TabIndex = 14;
            label2.Text = "Mô tả:";
            // 
            // label1
            // 
            label1.Location = new Point(6, 23);
            label1.Name = "label1";
            label1.Size = new Size(80, 25);
            label1.TabIndex = 12;
            label1.Text = "Tên vật tư:";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(616, 144);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(200, 39);
            txtSearch.TabIndex = 12;
            // 
            // txtlablesearch
            // 
            txtlablesearch.AutoSize = true;
            txtlablesearch.Location = new Point(616, 106);
            txtlablesearch.Name = "txtlablesearch";
            txtlablesearch.Size = new Size(279, 32);
            txtlablesearch.TabIndex = 14;
            txtlablesearch.Text = "Tìm kiếm theo tên vật tư";
            txtlablesearch.Click += label4_Click;
            // 
            // MaterialForm
            // 
            ClientSize = new Size(858, 718);
            Controls.Add(txtlablesearch);
            Controls.Add(txtSearch);
            Controls.Add(groupBox1);
            Controls.Add(groupBoxMaterials);
            Controls.Add(back);
            Name = "MaterialForm";
            Text = "Quản lý vật tư";
            groupBoxMaterials.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMaterials).EndInit();
            ((System.ComponentModel.ISupportInitialize)materialResponeMessageBindingSource).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nbUnits).EndInit();
            ((System.ComponentModel.ISupportInitialize)nbQuantity).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Button back;
		private GroupBox groupBox1;
		private Label label1;
		private TextBox txtDescription;
		private Label label2;
		private ToolTip toolTip1;
		private ComboBox cbbCategory;
		private Label label3;
		private BindingSource materialResponeMessageBindingSource;
		private NumericUpDown nbUnits;
		private NumericUpDown nbQuantity;
		private DataGridViewCheckBoxColumn materialCick;
		private DataGridViewTextBoxColumn materialIdDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn materialNameDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn weightDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn categoryNameDataGridViewTextBoxColumn;
        private TextBox txtSearch;
        private Label txtlablesearch;
    }
}

