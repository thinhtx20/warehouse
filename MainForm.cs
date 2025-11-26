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
			groupBox1 = new GroupBox();
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
			lblWelcome.Size = new Size(0, 21);
			lblWelcome.TabIndex = 0;
			// 
			// btnLogout
			// 
			btnLogout.BackColor = Color.LightCoral;
			btnLogout.ForeColor = Color.White;
			btnLogout.Location = new Point(679, 9);
			btnLogout.Name = "btnLogout";
			btnLogout.Size = new Size(100, 32);
			btnLogout.TabIndex = 1;
			btnLogout.Text = "Đăng xuất";
			btnLogout.UseVisualStyleBackColor = false;
			btnLogout.Click += btnLogout_Click;
			// 
			// btnReceipt
			// 
			btnReceipt.Location = new Point(63, 22);
			btnReceipt.Name = "btnReceipt";
			btnReceipt.Size = new Size(150, 50);
			btnReceipt.TabIndex = 2;
			btnReceipt.Text = "Nhập hàng";
			// 
			// btnIssue
			// 
			btnIssue.Location = new Point(278, 22);
			btnIssue.Name = "btnIssue";
			btnIssue.Size = new Size(150, 50);
			btnIssue.TabIndex = 3;
			btnIssue.Text = "Xuất hàng";
			btnIssue.Click += btnIssue_Click;
			// 
			// btnMaterials
			// 
			btnMaterials.Location = new Point(505, 22);
			btnMaterials.Name = "btnMaterials";
			btnMaterials.Size = new Size(150, 50);
			btnMaterials.TabIndex = 4;
			btnMaterials.Text = "Vật tư";
			// 
			// grbMenu
			// 
			grbMenu.Controls.Add(btnMaterials);
			grbMenu.Controls.Add(btnIssue);
			grbMenu.Controls.Add(btnReceipt);
			grbMenu.Location = new Point(33, 50);
			grbMenu.Name = "grbMenu";
			grbMenu.Size = new Size(749, 95);
			grbMenu.TabIndex = 6;
			grbMenu.TabStop = false;
			grbMenu.Text = "Menu";
			// 
			// grbMain
			// 
			grbMain.Controls.Add(dgvMaterials);
			grbMain.Location = new Point(20, 270);
			grbMain.Name = "grbMain";
			grbMain.Size = new Size(762, 377);
			grbMain.TabIndex = 7;
			grbMain.TabStop = false;
			grbMain.Text = "Danh sách vật tư";
			// 
			// dgvMaterials
			// 
			dgvMaterials.AutoGenerateColumns = false;
			dgvMaterials.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			dgvMaterials.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvMaterials.Columns.AddRange(new DataGridViewColumn[] { STT, materialIdDataGridViewTextBoxColumn, materialNameDataGridViewTextBoxColumn, unitDataGridViewTextBoxColumn, quantityDataGridViewTextBoxColumn, categoryNameDataGridViewTextBoxColumn });
			dgvMaterials.DataSource = materialResponeMessageBindingSource;
			dgvMaterials.Dock = DockStyle.Fill;
			dgvMaterials.Location = new Point(3, 19);
			dgvMaterials.Name = "dgvMaterials";
			dgvMaterials.RowHeadersWidth = 51;
			dgvMaterials.Size = new Size(756, 355);
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
			// groupBox1
			// 
			groupBox1.Location = new Point(33, 151);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new Size(749, 95);
			groupBox1.TabIndex = 7;
			groupBox1.TabStop = false;
			groupBox1.Text = "Thống kê kho";
			// 
			// MainForm
			// 
			BackColor = Color.FromArgb(240, 248, 255);
			ClientSize = new Size(791, 650);
			Controls.Add(groupBox1);
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
				this.Hide(); // ẩn MainForm
				var form = new ReceiptForm(_currentUser);
				form.ShowDialog();
				this.Show(); // hiện lại khi ReceiptForm đóng
			};

			btnIssue.Click += (s, e2) =>
			{
				this.Hide(); // ẩn MainForm
				var form = new IssueForm(_currentUser);
				form.ShowDialog();

				this.Show(); // hiện lại khi IssueForm đóng
			};

			btnMaterials.Click += (s, e2) =>
			{
				this.Hide(); // ẩn trước

				var form = new MaterialForm(_currentUser);
				form.ShowDialog();

				this.Show(); // hiện lại khi MaterialForm đóng
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

		private void btnLogout_Click(object sender, EventArgs e)
		{
			var form = new LoginForm();
			form.ShowDialog();
			this.Hide(); // ẩn MainForm
		}

		private void btnIssue_Click(object sender, EventArgs e)
		{

		}
	}
}
