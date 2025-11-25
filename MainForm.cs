using Inventory_manager;
using Inventory_manager.dto.Response;
using Inventory_manager.Models;
using Inventory_manager.Services;
using System.Threading.Tasks;
namespace Inventory_manager
{
	public partial class MainForm : Form
	{
		private User _currentUser;
		private Label lblWelcome;
		private Button btnLogout;
		private Button btnReceipt;
		private Button btnIssue;
		private Button btnMaterials;
		private Button btnWarehouses;
		private readonly MaterialServices _materialServices;
		private List<MaterialResponeMessage> _materialData = new List<MaterialResponeMessage>();
		public MainForm(User currentUser)
		{
			_currentUser = currentUser;
			InitializeComponent();
			_materialServices = new MaterialServices();
			this.Load += MainForm_Load;
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			lblWelcome = new Label();
			btnLogout = new Button();
			btnReceipt = new Button();
			btnIssue = new Button();
			btnMaterials = new Button();
			btnWarehouses = new Button();
			grbMenu = new GroupBox();
			grbMain = new GroupBox();
			dgvMaterials = new DataGridView();
			STT = new DataGridViewTextBoxColumn();
			materialIdDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			materialNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			unitDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			quantityDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			categoryNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
			materialResponeMessageBindingSource = new BindingSource(components);
			receiptRequestModelsBindingSource = new BindingSource(components);
			grbMenu.SuspendLayout();
			grbMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvMaterials).BeginInit();
			((System.ComponentModel.ISupportInitialize)materialResponeMessageBindingSource).BeginInit();
			((System.ComponentModel.ISupportInitialize)receiptRequestModelsBindingSource).BeginInit();
			SuspendLayout();
			// 
			// lblWelcome
			// 
			lblWelcome.AutoSize = true;
			lblWelcome.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
			lblWelcome.Location = new Point(20, 20);
			lblWelcome.Name = "lblWelcome";
			lblWelcome.Size = new Size(0, 28);
			lblWelcome.TabIndex = 0;
			// 
			// btnLogout
			// 
			btnLogout.BackColor = Color.LightCoral;
			btnLogout.ForeColor = Color.White;
			btnLogout.Location = new Point(711, 12);
			btnLogout.Name = "btnLogout";
			btnLogout.Size = new Size(100, 23);
			btnLogout.TabIndex = 1;
			btnLogout.Text = "Đăng xuất";
			btnLogout.UseVisualStyleBackColor = false;
			// 
			// btnReceipt
			// 
			btnReceipt.Location = new Point(40, 22);
			btnReceipt.Name = "btnReceipt";
			btnReceipt.Size = new Size(150, 50);
			btnReceipt.TabIndex = 2;
			btnReceipt.Text = "Nhập kho";
			// 
			// btnIssue
			// 
			btnIssue.Location = new Point(227, 22);
			btnIssue.Name = "btnIssue";
			btnIssue.Size = new Size(150, 50);
			btnIssue.TabIndex = 3;
			btnIssue.Text = "Xuất kho";
			// 
			// btnMaterials
			// 
			btnMaterials.Location = new Point(416, 22);
			btnMaterials.Name = "btnMaterials";
			btnMaterials.Size = new Size(150, 50);
			btnMaterials.TabIndex = 4;
			btnMaterials.Text = "Vật tư";
			// 
			// btnWarehouses
			// 
			btnWarehouses.Location = new Point(612, 22);
			btnWarehouses.Name = "btnWarehouses";
			btnWarehouses.Size = new Size(150, 50);
			btnWarehouses.TabIndex = 5;
			btnWarehouses.Text = "Thống kê kho";
			// 
			// grbMenu
			// 
			grbMenu.Controls.Add(btnWarehouses);
			grbMenu.Controls.Add(btnMaterials);
			grbMenu.Controls.Add(btnIssue);
			grbMenu.Controls.Add(btnReceipt);
			grbMenu.Location = new Point(33, 50);
			grbMenu.Name = "grbMenu";
			grbMenu.Size = new Size(778, 99);
			grbMenu.TabIndex = 6;
			grbMenu.TabStop = false;
			grbMenu.Text = "Menu";
			// 
			// grbMain
			// 
			grbMain.Controls.Add(dgvMaterials);
			grbMain.Location = new Point(20, 282);
			grbMain.Name = "grbMain";
			grbMain.Size = new Size(805, 377);
			grbMain.TabIndex = 7;
			grbMain.TabStop = false;
			grbMain.Text = "Danh sách vật tư";
			// 
			// dgvMaterials
			// 
			dgvMaterials.AllowUserToOrderColumns = true;
			dgvMaterials.AutoGenerateColumns = false;
			dgvMaterials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dgvMaterials.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvMaterials.Columns.AddRange(new DataGridViewColumn[] { STT, materialIdDataGridViewTextBoxColumn, materialNameDataGridViewTextBoxColumn, unitDataGridViewTextBoxColumn, quantityDataGridViewTextBoxColumn, categoryNameDataGridViewTextBoxColumn });
			dgvMaterials.DataSource = materialResponeMessageBindingSource;
			dgvMaterials.Location = new Point(6, 19);
			dgvMaterials.Name = "dgvMaterials";
			dgvMaterials.RowHeadersWidth = 51;
			dgvMaterials.Size = new Size(769, 331);
			dgvMaterials.TabIndex = 0;
			// 
			// STT
			// 
			STT.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			STT.HeaderText = "STT";
			STT.MinimumWidth = 6;
			STT.Name = "STT";
			// 
			// materialIdDataGridViewTextBoxColumn
			// 
			materialIdDataGridViewTextBoxColumn.DataPropertyName = "MaterialId";
			materialIdDataGridViewTextBoxColumn.HeaderText = "ID";
			materialIdDataGridViewTextBoxColumn.MinimumWidth = 6;
			materialIdDataGridViewTextBoxColumn.Name = "materialIdDataGridViewTextBoxColumn";
			// 
			// materialNameDataGridViewTextBoxColumn
			// 
			materialNameDataGridViewTextBoxColumn.DataPropertyName = "MaterialName";
			materialNameDataGridViewTextBoxColumn.HeaderText = "Tên";
			materialNameDataGridViewTextBoxColumn.MinimumWidth = 6;
			materialNameDataGridViewTextBoxColumn.Name = "materialNameDataGridViewTextBoxColumn";
			// 
			// unitDataGridViewTextBoxColumn
			// 
			unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
			unitDataGridViewTextBoxColumn.HeaderText = "Đơn vị";
			unitDataGridViewTextBoxColumn.MinimumWidth = 6;
			unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
			// 
			// quantityDataGridViewTextBoxColumn
			// 
			quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
			quantityDataGridViewTextBoxColumn.HeaderText = "Số Lượng";
			quantityDataGridViewTextBoxColumn.MinimumWidth = 6;
			quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
			// 
			// categoryNameDataGridViewTextBoxColumn
			// 
			categoryNameDataGridViewTextBoxColumn.DataPropertyName = "CategoryName";
			categoryNameDataGridViewTextBoxColumn.HeaderText = "Danh mục";
			categoryNameDataGridViewTextBoxColumn.MinimumWidth = 6;
			categoryNameDataGridViewTextBoxColumn.Name = "categoryNameDataGridViewTextBoxColumn";
			// 
			// materialResponeMessageBindingSource
			// 
			materialResponeMessageBindingSource.DataSource = typeof(MaterialResponeMessage);
			// 
			// receiptRequestModelsBindingSource
			// 
			receiptRequestModelsBindingSource.DataSource = typeof(dto.Request.ReceiptRequestModels);
			// 
			// MainForm
			// 
			BackColor = Color.FromArgb(240, 248, 255);
			ClientSize = new Size(862, 711);
			Controls.Add(grbMain);
			Controls.Add(grbMenu);
			Controls.Add(lblWelcome);
			Controls.Add(btnLogout);
			Name = "MainForm";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Quản lý kho vật tư";
			grbMenu.ResumeLayout(false);
			grbMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvMaterials).EndInit();
			((System.ComponentModel.ISupportInitialize)materialResponeMessageBindingSource).EndInit();
			((System.ComponentModel.ISupportInitialize)receiptRequestModelsBindingSource).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
		private async void MainForm_Load(object sender, EventArgs e)
		{
			lblWelcome.Text = $"Xin chào: {_currentUser.FullName}";

			btnLogout.Click += (s, e2) => Application.Restart();

			btnReceipt.Click += (s, e2) =>
			{
				var form = new ReceiptForm(_currentUser);
				form.ShowDialog();
				this.Hide(); // ẩn MainForm
			};

			btnIssue.Click += (s, e2) =>
			{
				var form = new IssueForm(_currentUser);
				form.ShowDialog();
				this.Hide(); // ẩn MainForm
			};

			btnMaterials.Click += (s, e2) =>
			{
				var form = new WarehouseForm();
				form.ShowDialog();
				this.Hide(); // ẩn MainForm
			};

			btnWarehouses.Click += (s, e2) =>
			{
				var form = new WarehouseForm();
				form.ShowDialog();
				this.Hide(); // ẩn MainForm 
			};
			await LoadMockData();
			LoadDataGridView();

		}
		private async Task LoadMockData()
		{
			_materialData = await _materialServices.GetMaterialsAsync(null);
		}
		private void LoadDataGridView()
		{
			dgvMaterials.DataSource = null;
			dgvMaterials.DataSource = _materialData;
		}

	}
}
